"""Aggregate explicit usage ledger records; never publish raw session contents."""
import argparse, collections, datetime, hashlib, json
from pathlib import Path

parser=argparse.ArgumentParser()
parser.add_argument('--sessions',required=True)
parser.add_argument('--root',required=True)
parser.add_argument('--cutoff',required=True)
parser.add_argument('--output',required=True)
parser.add_argument('--goal-tokens',type=int)
parser.add_argument('--goal-time-seconds',type=int)
a=parser.parse_args()
metas={}
for p in Path(a.sessions).rglob('*.jsonl'):
    try:
        with p.open('rb') as f: r=json.loads(f.readline())
    except (ValueError,OSError): continue
    if r.get('type')!='session_meta': continue
    d=r['payload']; source=d.get('source')
    spawn=source.get('subagent',{}).get('thread_spawn',{}) if isinstance(source,dict) else {}
    metas[d['id']]=(p,d,spawn)
selected={a.root}
while True:
    new={k for k,(_,d,s) in metas.items() if s.get('parent_thread_id') in selected}
    if new<=selected:break
    selected|=new
rows=[]; total=collections.Counter(); bymodel=collections.defaultdict(collections.Counter)
for ident in sorted(selected):
    p,d,spawn=metas[ident]; seen=set(); sums=collections.Counter(); models={}; current='unknown'
    duplicates=0; earliest=None; latest=None; last_cumulative=None; event_cumulative=None
    prefix=hashlib.sha256(); prefix_bytes=0; prompt_count=0; prompt_replies=0
    with p.open('rb') as f:
        for line in f:
            try:r=json.loads(line)
            except ValueError:continue
            stamp=r.get('timestamp','')
            if stamp>a.cutoff:break
            prefix.update(line);prefix_bytes+=len(line)
            x=r.get('payload',{}); kind=r.get('type')
            if kind=='turn_context':
                current=x.get('model','unknown'); models[x.get('turn_id')]=current
            if ident==a.root and kind=='response_item' and x.get('role')=='user':
                txt='\n'.join(c.get('text','') for c in x.get('content',[])).strip()
                if txt and not txt.startswith(('<environment_context>','<codex_internal_context','# AGENTS.md','<external_codex_apps_open_page>')):
                    prompt_count+=1;prompt_replies+=int(txt.startswith('<send_user_message_question_reply>'))
            if kind=='event_msg' and x.get('type')=='token_count':
                event_cumulative=(x.get('info') or {}).get('total_token_usage')
            if kind!='token_usage_record' or x.get('thread_id')!=ident:continue
            key=(ident,x.get('response_id'))
            if not key[1]:raise ValueError('Missing response identity')
            if key in seen:duplicates+=1;continue
            seen.add(key);u=x['usage']
            assert u['total_tokens']==u['input_tokens']+u['output_tokens']
            assert 0<=u.get('cached_input_tokens',0)<=u['input_tokens']
            assert 0<=u.get('reasoning_output_tokens',0)<=u['output_tokens']
            sums.update(u);bymodel[models.get(x.get('turn_id'),current)].update(u)
            earliest=earliest or stamp;latest=stamp;last_cumulative=x.get('thread_token_usage')
    if sums:
        sums['uncached_input_tokens']=sums['input_tokens']-sums['cached_input_tokens']
        total.update(sums)
    rows.append(dict(label=spawn.get('agent_path','root'),usage=dict(sums),uniqueResponses=len(seen),
        duplicateRecordsIgnored=duplicates,firstUsage=earliest,lastUsage=latest,
        cumulativeLedgerMatches=last_cumulative is not None and all(sums[k]==v for k,v in last_cumulative.items()),
        lastEventTokenCounter=event_cumulative if ident==a.root else None,
        sourcePrefixBytes=prefix_bytes,sourcePrefixSHA256=prefix.hexdigest(),
        rootHumanMessages=prompt_count if ident==a.root else None,rootQuestionReplies=prompt_replies if ident==a.root else None))
out=dict(schema='SfmPublicUsageLedger/v1',cutoffUTC=a.cutoff,timezone='Asia/Shanghai',
    scope='Available local root session and descendants identified by explicit parent_thread_id; only own-thread token_usage_record entries before cutoff.',
    method='Deduplicate by (thread_id,response_id); sum usage, never cumulative samples; cached input is a subset of input; reasoning is a subset of output. No double addition.',
    rootSessionStartedUTC=metas[a.root][1].get('timestamp'),sessions=len(rows),
    sessionsWithoutOwnLedger=sum(not r['uniqueResponses'] for r in rows),total=dict(total),
    byModelFromTurnContext={k:dict(v) for k,v in sorted(bymodel.items())},threads=rows,
    goalCounter=dict(tokensUsed=a.goal_tokens,timeUsedSeconds=a.goal_time_seconds,
        note='Optional caller-supplied separate goal snapshot; unknown if omitted. Scope/definition differ; do not add to session ledger or label as full-project usage.'),
    limitations=['Local telemetry, not an invoice or organization billing export.',
        'Missing/deleted/remote sessions and work outside this root subtree are not covered.',
        'Model attribution follows recorded turn context, not per-response billing metadata.',
        'event_msg token counters differ from ledger totals in this session; ledger is reported explicitly and both are retained, not summed.',
        'No exact dollar cost inferred without model/service-tier/cache-write pricing and billed usage reconciliation.'])
dest=Path(a.output);dest.parent.mkdir(parents=True,exist_ok=True)
with dest.open('x',encoding='utf-8') as f:json.dump(out,f,indent=2,ensure_ascii=False);f.write('\n')
print(json.dumps(dict(sessions=out['sessions'],missing=out['sessionsWithoutOwnLedger'],total=out['total'],root=next(r for r in rows if r['label']=='root')),ensure_ascii=False))

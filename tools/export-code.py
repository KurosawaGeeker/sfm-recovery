"""Export the current authority's C# source, with explicit resource exclusions."""
from pathlib import Path
import argparse, collections, hashlib, json, subprocess

p=argparse.ArgumentParser()
p.add_argument('--project',required=True)
p.add_argument('--destination',required=True)
p.add_argument('--source-commit',required=True)
p.add_argument('--snapshot-date',required=True)
a=p.parse_args()
source=Path(a.project).resolve(); dest=Path(a.destination).resolve()
assert (source/'ProjectSettings/ProjectVersion.txt').is_file()
assert (dest/'.git').is_dir(), 'Use a clean checkout of the public repository'
assert not (dest/'UnityProject').exists(), 'Fresh export required'
suffixes={'.cs','.asmdef','.asmref'}
files=[x for x in (source/'Assets').rglob('*') if x.is_file() and x.suffix.lower() in suffixes]
assert len(files)>1000
selected=set(files)
for f in files:
    meta=Path(str(f)+'.meta')
    if meta.is_file():selected.add(meta)
    for parent in f.parents:
        if parent==source:break
        meta=Path(str(parent)+'.meta')
        if meta.is_file():selected.add(meta)
selected.add(source/'ProjectSettings/ProjectVersion.txt')
selected.add(source/'Packages/manifest.json')
# The local lock points to embedded packages intentionally excluded here.
# The registry manifest is retained; consumers must resolve their own packages.
rows=[]
for f in sorted(selected):
    rel=f.relative_to(source).as_posix(); data=f.read_bytes()
    assert len(data)<1_000_000 and b'\x00' not in data, rel
    data.decode('utf-8-sig')
    target=dest/'UnityProject'/rel
    target.parent.mkdir(parents=True,exist_ok=True)
    with target.open('xb') as out:out.write(data)
    assert hashlib.sha256(f.read_bytes()).digest()==hashlib.sha256(data).digest()
    rows.append(dict(path='UnityProject/'+rel,bytes=len(data),sha256=hashlib.sha256(data).hexdigest()))
bydir=collections.Counter('/'.join(x['path'].split('/')[:4]) for x in rows if x['path'].endswith('.cs'))
manifest=dict(schema='SfmPublicSourceSnapshot/v1',sourceCommit=a.source_commit,
    snapshotDate=a.snapshot_date,authority='LayerFS sfm-all-v1/development',
    sourceBytesUnchanged=True,standaloneBuildable=False,gameplayComplete=False,
    csFiles=sum(x['path'].endswith('.cs') for x in rows),csBytes=sum(x['bytes'] for x in rows if x['path'].endswith('.cs')),
    files=len(rows),bytes=sum(x['bytes'] for x in rows),cSharpByDirectory=dict(bydir),
    exclusions=['Scenes','Prefabs','Meshes','Textures','Materials','Shaders','Animation clips/controllers','Audio','Fonts','DLL/EXE','Packages vendor contents','Library/cache','Saves','Credentials','Raw logs','Original Git history'],
    notes=['Snapshot of integrated source; uninstalled feedback-v4 and lightmap candidates excluded.',
           'No claim that every method has recovered native behavior; unresolved stubs remain.',
           'Static asset UI color edits are excluded along with all other serialized game assets.',
           'Original project package lock uses omitted embedded dependencies; restore licensed dependencies separately.'],
    inventory=rows)
out=dest/'docs/source-manifest.json';out.parent.mkdir(exist_ok=True)
with out.open('x',encoding='utf-8') as f:json.dump(manifest,f,indent=2,ensure_ascii=False);f.write('\n')
print(json.dumps({k:manifest[k] for k in ['csFiles','csBytes','files','bytes','standaloneBuildable']}))

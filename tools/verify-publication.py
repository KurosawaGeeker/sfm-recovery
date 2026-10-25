"""Validate the code-only snapshot. This is not a Unity or gameplay test."""
from pathlib import Path
import hashlib, json, re

ROOT=Path(__file__).resolve().parents[1]
manifest=json.loads((ROOT/'docs/source-manifest.json').read_text(encoding='utf-8'))
expected={r['path']:r for r in manifest['inventory']}
assert len(expected)==manifest['files']
actual={p.relative_to(ROOT).as_posix() for p in (ROOT/'UnityProject').rglob('*') if p.is_file()}
assert actual==set(expected), 'Unexpected or missing Unity files'
for name,row in expected.items():
    p=ROOT/name
    assert p.resolve().is_relative_to(ROOT)
    b=p.read_bytes()
    assert len(b)==row['bytes'] and hashlib.sha256(b).hexdigest()==row['sha256'],name
    b.decode('utf-8-sig')
    assert b'\0' not in b,name
    assert p.suffix in {'.cs','.meta','.asmdef','.asmref','.json','.txt'},name
    if p.suffix=='.meta':
        target=Path(str(p)[:-5])
        assert target.is_dir() or target.relative_to(ROOT).as_posix() in expected,name
forbidden={'.dll','.exe','.pdb','.png','.jpg','.jpeg','.dds','.fbx','.obj','.wav','.ogg','.mp3',
           '.mp4','.ttf','.otf','.unity','.prefab','.asset','.mat','.anim','.controller','.shader',
           '.bytes','.dat','.sd','.zip','.7z','.log','.pem','.key','.pfx','.p12','.sqlite','.jsonl'}
secret=re.compile(r'(?<![A-Za-z0-9_-])(?:gh[pousr]_[A-Za-z0-9]{30,}|github_pat_[A-Za-z0-9_]{30,}|sk-(?:proj-)?[A-Za-z0-9_-]{30,}|AKIA[0-9A-Z]{16})')
scanned=0
for p in ROOT.rglob('*'):
    if not p.is_file() or '.git' in p.relative_to(ROOT).parts:continue
    rel=p.relative_to(ROOT)
    if '__pycache__' in rel.parts:continue
    assert p.suffix.lower() not in forbidden,str(rel)
    assert not any(x in rel.parts for x in ['Library','Temp','SaveData','SaveDataBackup','UserSettings']),str(rel)
    assert not p.name.startswith('.env'),str(rel)
    text=p.read_text(encoding='utf-8-sig')
    assert not secret.search(text),'Credential-like token in '+str(rel)
    # Concatenate to avoid this scanner matching its own source literal.
    assert not re.search('-----BEGIN '+r'(?:RSA |EC |OPENSSH )?PRIVATE KEY-----',text),str(rel)
    scanned+=1
usage=json.loads((ROOT/'docs/usage-summary.json').read_text(encoding='utf-8'))
keys=['input_tokens','cached_input_tokens','output_tokens','total_tokens','reasoning_output_tokens']
for key in keys:
    assert sum(r['usage'].get(key,0) for r in usage['threads'])==usage['total'][key]
    assert sum(r.get(key,0) for r in usage['byModelFromTurnContext'].values())==usage['total'][key]
assert all(r['cumulativeLedgerMatches'] for r in usage['threads'])
assert usage['total']['total_tokens']==usage['total']['input_tokens']+usage['total']['output_tokens']
print(json.dumps(dict(passed=True,sourceFiles=len(expected),csFiles=manifest['csFiles'],
                     allPublicFilesScanned=scanned,staticAssetPayloads=0,unityBuildTested=False,
                     note='Pattern scan plus explicit file allowlist; not an exhaustive security or gameplay proof.')))

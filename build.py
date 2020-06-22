#! /usr/bin/env python3

import os
import os.path
import re
import shutil
import subprocess

projects = [
    'Kwyjibo',
    'Kwyjibo.Extensions.DependencyInjection'
]

version = '0.1.0'
suffix = 'beta'
is_release = False


def abspath(path):
    home = os.path.abspath(os.path.dirname(__file__))
    return os.path.normpath(os.path.join(home, path))


def dotnet(*args):
    process = ["dotnet"] + list(args)
    subprocess.run(process, check=True)


build_number = os.environ.get('APPVEYOR_BUILD_NUMBER', '0')
pull_request_number = os.environ.get('APPVEYOR_PULL_REQUEST_NUMBER', False)

if os.environ.get('APPVEYOR_REPO_TAG', False) == 'true':
    is_release = True

version_number = re.search(r'^\d+\.\d+\.\d+', version)
if version_number:
    version_number = version_number.group()
    file_version = version_number + '.' + build_number
else:
    file_version = False

if pull_request_number:
    package_version = version + '-pr' + pull_request_number
else:
    package_version = version
    if suffix and suffix != '':
        package_version += '-' + suffix

package_path = abspath('build')

os.makedirs(abspath('src/.version'), exist_ok=True)
with open(abspath('src/.version/version.cs'), 'w') as f:
    f.writelines([
        'using System.Reflection;\n'
        '\n',
        '[assembly:AssemblyInformationalVersion("{0}")]\n'.format(package_version)
    ])
    if file_version:
        f.writelines([
            '[assembly:AssemblyVersion("{0}")]\n'.format(file_version),
            '[assembly:AssemblyFileVersion("{0}")]\n'.format(file_version)
        ])

shutil.rmtree(abspath('build'), ignore_errors=True)
dotnet('build', abspath('src/Kwyjibo.sln'))
dotnet('test', abspath('src/Kwyjibo.Tests/Kwyjibo.Tests.csproj'))

for project in projects:
    dotnet(
        'pack',
        '-o', package_path,
        '--no-build',
        '-p:PackageVersion=' + package_version,
        abspath('src/{0}/{0}.csproj'.format(project))
    )

if is_release:
    key = os.environ.get('NUGET_KEY', False)
    if key:
        for f in os.listdir(package_path):
            artifact = os.path.join(package_path, f)
            if artifact.endswith('.nupkg') and os.path.isfile(artifact):
                dotnet(
                    'nuget', 'push', artifact,
                    '-k', key,
                    '-s', 'https://api.nuget.org/v3/index.json'
                )

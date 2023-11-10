const manifestFileName = process.argv[2];

let binFolderName = "bin"
let argBuildVersion = null

if (process.argv.length > 3) {
    for (const val of process.argv.slice(3)) {
        if (val.startsWith("binFolderName")) {
            binFolderName = val.substring("binFolderName=".length)
        } else {
            argBuildVersion = val;
        }
    }
}


// @ts-ignore
const manifestFile = require('./' + binFolderName + '/' + manifestFileName);
const dasherize = require('dasherize');
const fs = require('fs');

const builderConfiguration = {...manifestFile.build};

if (argBuildVersion !== null) {
    builderConfiguration.buildVersion = argBuildVersion;
}

if (builderConfiguration.hasOwnProperty('buildVersion')) {
    // @ts-ignore
    const packageJson = require('./package');
    packageJson.name = dasherize(manifestFile.name || 'electron-sharp');
    packageJson.author = manifestFile.author || '';
    packageJson.version = builderConfiguration.buildVersion;
    packageJson.description = manifestFile.description || '';

    fs.writeFile('./package.json', JSON.stringify(packageJson), (error) => {
        if (error) {
            console.log(error.message);
        }
    });

    try {
        // @ts-ignore
        const packageLockJson = require('./package-lock');
        packageLockJson.name = dasherize(manifestFile.name || 'electron-sharp');
        packageLockJson.author = manifestFile.author || '';
        packageLockJson.version = builderConfiguration.buildVersion;
        fs.writeFile('./package-lock.json', JSON.stringify(packageLockJson), (error) => {
            if (error) {
                console.log(error.message);
            }
        });
    } catch (error) {
        // ignore missing module
    }
}

const builderConfigurationString = JSON.stringify(builderConfiguration);
fs.writeFile('./' + binFolderName + '/electron-builder.json', builderConfigurationString, (error) => {
    if (error) {
        console.log(error.message);
    }
});

const manifestContent = JSON.stringify(manifestFile);
fs.writeFile('./' + binFolderName + '/electron.manifest.json', manifestContent, (error) => {
    if (error) {
        console.log(error.message);
    }
});
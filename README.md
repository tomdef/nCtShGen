# nCtShGen
**C**on**t**act**Sh**eet **Gen**erator creates preview of all images from specified folder(s).

## Usage ##

To generate contactsheet(s) :
1) configure appsettings.json
2) just run ```.\nCtShGen.exe``` with command ```generate```

## How to configure application ##
```
{
    "appsettings": {
        "rootPhotoFolder": "<main folder with images>",
        "excludeFolders": "<folders in rootPhotoFolder to exclude>",
        "contactSheetFolder": "<result folder>",
        "contactSheetFileNameTemplate": "<output image name template>",
        "contactSheetRootFolderOnLevel": <generate contactsheet for folder(s) on this level. (0 means that generate contactsheet for root folder)>,
        "contactSheetSubfolderDeepLevel": <how deep program should list files. (2-3 are good values)>,
        "contactSheetStyle": "<conctactsheet color schema. (Available values : Dark/Light)>",
        "contactSheetExistsAction": "<what program should do if output contactsheet image exists (Available values : Skip/Override/Ask)>",
        "thumbnail": {
            "maxWidth": <width of contactsheet item>,
            "maxHeight": <height of contactsheet item>
        }
    }
}
```

YUI.add("yuidoc-meta", function(Y) {
   Y.YUIDoc = { meta: {
    "classes": [
        "Formatter",
        "Formatter.date",
        "Formatter.number",
        "Formatter.util",
        "Formatter.util.date",
        "Formatter.util.number",
        "cultures.deAT",
        "cultures.deDE",
        "cultures.enUS",
        "flavors.java",
        "flavors.java.Formatter",
        "flavors.java.Formatter.number",
        "flavors.java.Formatter.number.localize",
        "flavors.java.Formatter.util",
        "flavors.java.Parser",
        "flavors.java.Scanner",
        "flavors.java.Scanner.tokenCategories"
    ],
    "modules": [
        "cultures",
        "flavors",
        "pat"
    ],
    "allModules": [
        {
            "displayName": "cultures",
            "name": "cultures",
            "description": "Culture specific information."
        },
        {
            "displayName": "flavors",
            "name": "flavors",
            "description": "Formatting data described by format specifiers of a certain flavor."
        },
        {
            "displayName": "pat",
            "name": "pat",
            "description": "Represents a data formatter. Data to be formatted is described by format\nspecifiers of a certain flavor.\n\nSupported format specifiers:\n\n+    Java (java.util.Formatter)"
        }
    ]
} };
});
Sitecore-Dialog-Fix
===================

With the release of Google Chrome 37 we have finally lost our beloved modal dialogs created with showModalDialog(). This is used in all versions of Sitecore up to version 7.1. There is no official workaround for this yet, aside from use a different browser or upgrade.

This patch back-ports the functionality of Sitecore 7.1 to use the extended version of the jQuery UI Dialog plugin. Due to a compatibility issue with Internet Explorer there is conditional checks in the code so IE uses the existing Sitecore functionality instead (since this should continue to work for the forseeable future... we hope).

Copy the release folder into your Sitecore installation. If you are using Sitecore 6.5 then rename the Gecko.SC65.js and Sitecore.SC65.js files.

If everything fails then revert Gecko.js and Sitecore.js to the original files (included in /Source/original-js-files) and delete the the patch include file.

I've tested in: Internet Explorer 11, Firefox 31 and Google Chrome 36 + 37.
In following version of Sitecore: 6.5 rev 121009 (SP2), 6.6 rev 140410 (SP2), 7.0 rev 140408 (SP1).

These have all been vanilla installs, and have not seen the light of day of a QA server let alone a production server. Usual disclaimer, use at your own risk. This is highly experimental and not in any way officially endorsed by Sitecore.

More info in my blog post: http://wp.me/p2SmN4-2z

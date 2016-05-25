# WOPIForms
Working example of the project can be seen from the Collab365 Summit 2016 Session titled "Building Your Own Web-Based Froms Solution in SharePoint 2013 - 2016"
https://collab365.conferencehosts.com/confs/Summit2016/c365summitbulutay/

WOPIForms is an experimental project which gets use of the Web Application Open Interface that is available in SharePoint 2013 and SharePoint 2016 in order to create a web based viewer for XML typed files in SharePoint libraries.

Funtionality of WOPIForms is pretty straight-forward for ones who are familiar with XML transformation:

1- It reads the XML file which the user clicks on

2- It transforms the XML file to HTML using the specified XSLT

3- It renders the transformed HTML as a form to be filled in.

//TODO:

1- Adding reverse transform capability to retransform the filled in XML form and save it to SharePoint.

2- Validating the version of form with XSD.

3- Getting XSLT and XSD files from the /Forms folder of the related library.

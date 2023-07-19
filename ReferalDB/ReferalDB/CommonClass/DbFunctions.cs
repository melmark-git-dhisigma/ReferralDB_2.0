using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Objects.SqlClient;
using System.Xml;
using System.Web.UI.WebControls;
using DataLayer;
using ReferalDB.Models;
using ReferalDB;
using System.Text;
using System.Data;


using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;

using Microsoft.Office.Core;
using System.IO.Packaging;
using DocumentFormat.OpenXml.Packaging;


namespace ReferalDB.CommonClass
{

    public class DbFunctions
    {
        MelmarkDBEntities Objdata = null;

        public int Login(LoginModel model)
        {
            Objdata = new MelmarkDBEntities();
            int Userid = 0;


            var query = from a in Objdata.Users select a;
            var data = query.ToList();
            var result = from a in data
                         select new
                         {
                             Uname = Encoding.UTF8.GetString(a.Login),
                             Pass = Encoding.UTF8.GetString(a.Password),
                             Status = a.ActiveInd,
                             UserID = a.UserId,
                             Schoolid=a.SchoolId
                         };

            var User = (from obj in result where obj.Uname == model.UserName && obj.Pass == model.Password && obj.Status == "A" select new
                         {
                             UserID = obj.UserID,
                             Schoolid = obj.Schoolid
                         }).FirstOrDefault();
            if (User != null)
                Userid = User.UserID;
            return Userid;
        }



        /*********************************************************************************/
        /******************************| Digital Signature |******************************/
        /*********************************************************************************/


        static readonly string RT_OfficeDocument = "http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument";
        static readonly string OfficeObjectID = "idOfficeObject";
        static readonly string SignatureID = "idPackageSignature";
        static readonly string ManifestHashAlgorithm = "http://www.w3.org/2000/09/xmldsig#sha1";

        //private Microsoft.Office.Interop.Word.Application WordApp = new Microsoft.Office.Interop.Word.Application();
        //public string SignDocument(int StudentId, int DocumentId)
        //{
        //    string file = @"E:\DsignatureDoc\SampleTest.docx";

        //    bool checkInd = false;
        //    XmlDocument xml = new XmlDocument();
        //    using (var document = WordprocessingDocument.Open(file, false))
        //    {
        //        xml.Load(HttpContext.Current.Server.MapPath("../XML/OfficeObject.xml"));
        //        var signature = document.MainDocumentPart.Document.Descendants<DocumentFormat.OpenXml.Vml.Office.SignatureLine>().ToList();
        //        foreach (var sign in signature)
        //        {
        //            var suggestedSigner = sign.SuggestedSigner;
        //            if (suggestedSigner == "Parent")
        //            {
        //                XmlNodeList setupId = xml.GetElementsByTagName("SetupID");
        //                XmlNodeList signText = xml.GetElementsByTagName("SignatureText");
        //                if (setupId != null)
        //                {
        //                    if (setupId.Count > 0)
        //                    {
        //                        setupId[0].InnerText = sign.Id.ToString();
        //                    }
        //                }
        //                if (signText != null)
        //                {
        //                    if (signText.Count > 0)
        //                    {
        //                        signText[0].InnerText = "Parent Name";
        //                    }
        //                }
        //                xml.Save(HttpContext.Current.Server.MapPath("../XML/OfficeObject.xml"));
        //                checkInd = true;
        //            }
        //        }
        //        document.Close();
        //    }
        //    if (!checkInd) return "Document does not contain Signature Line for Parent";
        //    else return DigiSign(file, xml);
        //}

        //public string DigiSign(string Filename, XmlDocument xdoc)
        //{
        //    // Open the Package    
        //    using (Package package = Package.Open(Filename))
        //    {
        //        // Get the certificate
        //        X509Certificate2 certificate = GetCertificate();
        //        string rtrn = SignAllParts(package, certificate, xdoc);
        //        package.Close();
        //        return rtrn;
        //    }

        //}

        //public string SignAllParts(Package package, X509Certificate certificate, XmlDocument xdoc)
        //{
        //    if (package == null) throw new ArgumentNullException("SignAllParts(package)");
        //    List<Uri> PartstobeSigned = new List<Uri>();
        //    List<PackageRelationshipSelector> SignableReleationships = new List<PackageRelationshipSelector>();

        //    foreach (PackageRelationship relationship in package.GetRelationshipsByType(RT_OfficeDocument))
        //    {
        //        // Pass the releationship of the root. This is decided based on the RT_OfficeDocument (http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument)
        //        CreateListOfSignableItems(relationship, PartstobeSigned, SignableReleationships);
        //    }
        //    // Create the DigitalSignature Manager
        //    PackageDigitalSignatureManager dsm = new PackageDigitalSignatureManager(package);
        //    dsm.CertificateOption = CertificateEmbeddingOption.InSignaturePart;


        //    string signatureID = SignatureID;
        //    string manifestHashAlgorithm = ManifestHashAlgorithm;
        //    System.Security.Cryptography.Xml.DataObject officeObject = CreateOfficeObject(signatureID, manifestHashAlgorithm, xdoc);
        //    Reference officeObjectReference = new Reference("#" + OfficeObjectID);

        //    try
        //    {
        //        dsm.Sign(PartstobeSigned, certificate, SignableReleationships, signatureID, new System.Security.Cryptography.Xml.DataObject[] { officeObject }, new Reference[] { officeObjectReference });
        //        return "Success";
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message;
        //    }






        //}// end:SignAllParts()

        ///**************************SignDocument******************************/
        ////  This function is a helper function. The main role of this function is to 
        ////  create two lists, one with Package Parts that you want to sign, the other 
        ////  containing PacakgeRelationshipSelector objects which indicate relationships to sign.
        ///*******************************************************************/
        //public void CreateListOfSignableItems(PackageRelationship relationship, List<Uri> PartstobeSigned, List<PackageRelationshipSelector> SignableReleationships)
        //{
        //    // This function adds the releation to SignableReleationships. And then it gets the part based on the releationship. Parts URI gets added to the PartstobeSigned list.
        //    PackageRelationshipSelector selector = new PackageRelationshipSelector(relationship.SourceUri, PackageRelationshipSelectorType.Id, relationship.Id);
        //    SignableReleationships.Add(selector);
        //    if (relationship.TargetMode == TargetMode.Internal)
        //    {
        //        PackagePart part = relationship.Package.GetPart(PackUriHelper.ResolvePartUri(relationship.SourceUri, relationship.TargetUri));
        //        if (PartstobeSigned.Contains(part.Uri) == false)
        //        {
        //            PartstobeSigned.Add(part.Uri);
        //            // GetRelationships Function: Returns a Collection Of all the releationships that are owned by the part.
        //            foreach (PackageRelationship childRelationship in part.GetRelationships())
        //            {
        //                CreateListOfSignableItems(childRelationship, PartstobeSigned, SignableReleationships);
        //            }
        //        }
        //    }
        //}
        ///**************************SignDocument******************************/
        ////  Once you create the list and try to sign it, Office will not validate the Signature.
        ////  To allow Office to validate the signature, it requires a custom object which should be added to the 
        ////  signature parts. This function loads the OfficeObject.xml resource.
        ////  Please note that GUID being passed in document.Loadxml. 
        ////  Background Information: Once you add a SignatureLine in Word, Word gives a unique GUID to it. Now while loading the
        ////  OfficeObject.xml, we need to make sure that The this GUID should match to the ID of the signature line. 
        ////  So if you are generating a SignatureLine programmtically, then mmake sure that you generate the GUID for the 
        ////  SignatureLine and for this element. 
        ///*******************************************************************/

        //public System.Security.Cryptography.Xml.DataObject CreateOfficeObject(string signatureID, string manifestHashAlgorithm, XmlDocument document)
        //{
        //    //XmlDocument document = new XmlDocument();
        //    //document.LoadXml(HttpContext.Current.Server.MapPath("../Documents/XML/OfficeObject.xml"));
        //    System.Security.Cryptography.Xml.DataObject officeObject = new System.Security.Cryptography.Xml.DataObject();
        //    // do not change the order of the following two lines
        //    officeObject.LoadXml(document.DocumentElement); // resets ID
        //    officeObject.Id = OfficeObjectID; // required ID, do not change
        //    return officeObject;
        //}
        ///********************************************************/

        //public X509Certificate2 GetCertificate()
        //{
        //    // X509Store certStore = new X509Store(StoreLocation.CurrentUser);     //for local build

        //    X509Store certStore = new X509Store(StoreLocation.LocalMachine);    //for server IIS Build
        //    certStore.Open(OpenFlags.ReadOnly);
        //    //X509Certificate2Collection certs = X509Certificate2UI.SelectFromCollection(certStore.Certificates, "Select a certificate", "Please select a certificate",
        //    //        X509SelectionFlag.SingleSelection);
        //    X509Certificate2Collection certs = certStore.Certificates.Find(X509FindType.FindByIssuerName, "localhost", false);
        //    return certs.Count > 0 ? certs[0] : null;
        //}

        /*********************************************************************************/
        /************************|  Digital Signature End  |******************************/
        /*********************************************************************************/
        
    }
}
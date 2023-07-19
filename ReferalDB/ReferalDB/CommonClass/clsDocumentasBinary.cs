using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer;
using ReferalDB.Models;
using System.Web.Mvc;
using System.Data.Objects.SqlClient;
using System.Transactions;
using System.IO;
using System.Xml;
using System.Data.SqlClient;
using System.Web.UI.HtmlControls;




//29/5/2014 Neethu
namespace ReferalDB.CommonClass
{
    public class clsDocumentasBinary
    {
        public MelmarkDBEntities objData = null;

        public byte[] getBinaryFile(HtmlInputFile Upfile)
        {
            string fileName = Path.GetFileName(Upfile.PostedFile.FileName);
            byte[] bytes = null;
            string filename = Path.GetFileName(Upfile.PostedFile.FileName);


            using (Stream fs = Upfile.PostedFile.InputStream)
            {
                using (BinaryReader br = new BinaryReader(fs))
                {
                    bytes = br.ReadBytes((Int32)fs.Length);
                }
            }
            return bytes;
        }


        public string LookName(int LookUpId)
        {
            objData = new MelmarkDBEntities();
            string LookUpN = ((from LookUp lukup in objData.LookUps
                               where (lukup.LookupId == LookUpId)
                               select new
                               {
                                   Name = lukup.LookupName

                               }).SingleOrDefault()).Name;

            return LookUpN;

        }

        public void SaveBinaryFiles(int SchoolId, int StudentId, string fileName, int UserId, HttpPostedFileBase Upfile, string Type, string MName, int Docid)
        {

            try
            {

                int rtrnval = -1;
                string contentType = Upfile.ContentType;



                byte[] bytes = null;
                using (Stream fs = Upfile.InputStream)
                {
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        bytes = br.ReadBytes((Int32)fs.Length);
                    }
                }
                objData = new MelmarkDBEntities();

                binaryFile binfile = new binaryFile();
                binfile.SchoolId = SchoolId;
                binfile.StudentId = StudentId;
                binfile.DocumentName = fileName;
                binfile.DocId = Docid;
                binfile.ContentType = contentType;
                binfile.Data = bytes;
                binfile.CreatedBy = UserId;
                binfile.CreatedOn = DateTime.Now;
                //  binfile.ContentType = contentType;

                if((Type=="Referal_upld")&&(MName=="Funding"||MName=="IEP"||MName=="Placement Agreement"||MName=="Contract"||MName=="Consent"))
                    binfile.Varified = false;
                else if ((Type == "Referal_upld") && (MName != "Funding" || MName != "IEP" || MName != "Placement Agreement" || MName != "Contract" || MName != "Consent"))
                 binfile.Varified = true;
                else
                    binfile.Varified = false;
                if (Type == "Referal_upld")
                    Type = "Referal";
                binfile.type = Type;
                binfile.ModuleName = MName;
                objData.binaryFiles.Add(binfile);
                objData.SaveChanges();
                rtrnval = binfile.BinaryId;

            }
            catch
            {
            }





        }



    }
}
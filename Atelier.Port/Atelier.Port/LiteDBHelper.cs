using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atelier.Port.Models;
using LiteDB;

namespace Atelier.Port
{
   public class LiteDBHelper
    {
        protected LiteCollection<Identification_Remorque> RemorqueCollection;
        private LiteCollection<ItemRemorque> EmailsCollection;
        private LiteCollection<Personel> PersonelCollection;
       // protected bool PersonDrop;
       // protected bool RemorqueDrop;
       // protected bool EmailsDrop;
        public LiteDBHelper(string dbPath)
        {
            using (var db = new LiteDatabase(dbPath))
            {
                RemorqueCollection = db.GetCollection<Identification_Remorque>("Remorques");
                EmailsCollection = db.GetCollection<ItemRemorque>("EmailsDrop");
                PersonelCollection = db.GetCollection<Personel>("Personel");
                
                     //PersonDrop = db.DropCollection("Personel");
                     //RemorqueDrop = db.DropCollection("Remorques");
                    // RemorqueDrop = db.DropCollection("EmailsDrop");}
                     
                
            }
            }
        //Drop All Database


        public  void  DropAll()
        {
            
            foreach (var item in GetAllUser())
            {
                PersonelCollection.Delete(a => a._id == item._id);
            }
            foreach (var item in GetAllEmails())
            {
                EmailsCollection.Delete(a => a._id == item._id);
            }
            foreach (var item in GetAllRemorque())
            {
                RemorqueCollection.Delete(a => a._id == item._id);
            }
           
        }

        //Add user
        public void AddPersonel(Personel refre)
        {

            PersonelCollection.Insert(refre);
        }
        //get all user
        public List<Personel> GetAllUser()
        {
            return new List<Personel>(PersonelCollection.FindAll());
            
        }
        //delete remorque
        public void DelateEmails(Guid id)
        {
            EmailsCollection.Delete (a => a._id == id);
        }
        //add remorque 
        public void AddEmails(ItemRemorque materiel1)
        {
            EmailsCollection.Insert(materiel1);
        }

        //updat item remorque
        public void UpdatEmails(ItemRemorque materiel1)
        {
            EmailsCollection.Update(materiel1);
        }
        //Get All Referances Materiels 
        public List<ItemRemorque> GetAllEmails()
        {
            var Remorque_List = new List<ItemRemorque>(EmailsCollection.FindAll());
            return Remorque_List;
        }

        //Get All Referances Materiels 
        public List<Identification_Remorque> GetAllRemorque()
        {
            var Remorque_List = new List<Identification_Remorque>(RemorqueCollection.FindAll());
            return Remorque_List;
        }
        //add remorque 
        public void AddRemorque(Identification_Remorque materiel)
        {
            RemorqueCollection.Insert(materiel);
        }
        //Get All Referances Materiels 
        public string[] GetNbRemorque()
        { 
            var Remorque_List = new List<Identification_Remorque>(RemorqueCollection.FindAll());
            List<string> Array_Remorque = new List<string>() ;
            foreach (var c in Remorque_List)
            {
                Array_Remorque.Add(c.Numero_Remorque);
            }
            return Array_Remorque.ToArray();
        }
        //Get email client par nb Remorque 
        public string GetEmailByNbRemorque(string N_remorque)
        {
            var Remorque_List = new List<Identification_Remorque>(RemorqueCollection.FindAll());
            var val = "";
            foreach (var c in Remorque_List)
            {
                if (c.Numero_Remorque == N_remorque)
                {
                    val = c.Email_Client;
                }  
            }
            return val;
        }
        //Get nom client par nb Remorque 
        public string GetNomClientByNbRemorque(string N_remorque)
        {
            var Remorque_List = new List<Identification_Remorque>(RemorqueCollection.FindAll());
            var val = "";
            foreach (var c in Remorque_List)
            {
                if (c.Numero_Remorque == N_remorque)
                {
                    val = c.Nom_Client;
                }
            }
            return val;
        }
        //Get etat envoi email par nb Remorque 
       /* public bool GetEtatEmailByNbRemorque(string N_remorque)
        {
            var Remorque_List = new List<Identification_Remorque>(RemorqueCollection.FindAll());
            bool val = false;
            foreach (var c in Remorque_List)
            {
                if (c.Numero_Remorque == N_remorque)
                {
                    val = c.Choix_email;
                }
            }
            return val;
        }*/
    }
}

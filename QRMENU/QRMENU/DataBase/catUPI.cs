using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QRMENU.Models;

namespace QRMENU
{
    public class catUPI
    {
        public void catDelete(int id)
        {
            using (qrpandemenu_Entities del = new qrpandemenu_Entities())
            {
                var kt = del.kategoriler.Find(id);
                del.kategoriler.Remove(kt);
                del.SaveChanges();
            }
        }
    }
}
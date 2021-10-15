using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QRMENU.Models;

namespace QRMENU
{
    public class prodUDI
    {
        public void proDelete(int id)
        {
            using (qrpandemenu_Entities z = new qrpandemenu_Entities())
            {
                var urn = z.urunler.Find(id);
                z.urunler.Remove(urn);
                z.SaveChanges();
            }
        }
    }
}
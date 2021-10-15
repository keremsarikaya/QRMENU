using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QRMENU.Models;

namespace QRMENU
{
    public class ResimEkleme
    {
        public void resimekle (string resimad, int urunid)
        {
            using (qrpandemenu_Entities c = new qrpandemenu_Entities())
            {
                resim rsm = new resim();
                rsm.resimad = resimad;
                rsm.durum = true;
                rsm.urunid = urunid;
                c.resim.Add(rsm);
                c.SaveChanges();

            }
        }
        public void catresimEkle(string k_resimad, int kategori_id)
        {
            using (qrpandemenu_Entities x = new qrpandemenu_Entities())
            {
                kategori_resim kresim = new kategori_resim();
                kresim.k_resimad = k_resimad;
                kresim.k_durum = true;
                kresim.kategori_id = kategori_id;
                x.kategori_resim.Add(kresim);
                x.SaveChanges();
            }
        }
        public void branchimgAdd(string brancimg, int branchid)
        {
            using (qrpandemenu_Entities z = new qrpandemenu_Entities())
            {
                subeLogo bimg = new subeLogo();
                bimg.logoad = brancimg;
                bimg.durum = true;
                bimg.subeid = branchid;
                z.subeLogo.Add(bimg);
                z.SaveChanges();
            }
        }
    }
}
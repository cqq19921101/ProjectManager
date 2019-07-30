using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;
namespace LiteOn.GDS.Utility
{
   public class Tools
    {
        /// <summary>
        /// datatable convert to xml string
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        static public string DataTableToXML(DataTable dt)
        {
            StringWriter sw = new StringWriter();
            dt.WriteXml(sw);
            string temp = sw.ToString();
            sw.Close();
            sw.Dispose();
            return temp;
        }

        /// <summary>
        /// 存儲表頭
        /// </summary>
        /// <returns></returns>
        static public DataTable BuildHeadTable()
        {
            DataTable dt = new DataTable("T_GDS_HEAD");
            dt.Columns.Add(new DataColumn("WERKS", typeof(String)));
            dt.Columns.Add(new DataColumn("MBLNR_A", typeof(String)));
            dt.Columns.Add(new DataColumn("MJAHR_A", typeof(String)));
            dt.Columns.Add(new DataColumn("APTYP", typeof(String)));
            dt.Columns.Add(new DataColumn("KOKRS", typeof(String)));
            dt.Columns.Add(new DataColumn("KOSTL", typeof(String)));
            dt.Columns.Add(new DataColumn("ABTEI", typeof(String)));
            dt.Columns.Add(new DataColumn("APPER", typeof(String)));
            dt.Columns.Add(new DataColumn("WERKF", typeof(String)));
            dt.Columns.Add(new DataColumn("WERKT", typeof(String)));
            dt.Columns.Add(new DataColumn("LOCFM", typeof(String)));
            dt.Columns.Add(new DataColumn("LOCTO", typeof(String)));
            dt.Columns.Add(new DataColumn("BWART", typeof(String)));
            dt.Columns.Add(new DataColumn("GRUND", typeof(String)));
            dt.Columns.Add(new DataColumn("TCODE", typeof(String)));
            dt.Columns.Add(new DataColumn("HKONT", typeof(String)));
            dt.Columns.Add(new DataColumn("ERDAT", typeof(String)));
            dt.Columns.Add(new DataColumn("ERZEI", typeof(String)));
            dt.Columns.Add(new DataColumn("ERNAM", typeof(String)));
            dt.Columns.Add(new DataColumn("MBLNR", typeof(String)));
            dt.Columns.Add(new DataColumn("MJAHR", typeof(String)));
            dt.Columns.Add(new DataColumn("BUDAT", typeof(String)));
            dt.Columns.Add(new DataColumn("REMARK", typeof(String)));
            dt.Columns.Add(new DataColumn("PRTNM", typeof(String)));
            dt.Columns.Add(new DataColumn("RTNIF", typeof(String)));
            dt.Columns.Add(new DataColumn("AUFNR", typeof(String)));
            dt.Columns.Add(new DataColumn("RTNDT", typeof(String)));
            dt.Columns.Add(new DataColumn("UPDOK", typeof(String)));
            dt.Columns.Add(new DataColumn("APROV", typeof(String)));
            dt.Columns.Add(new DataColumn("GAMNG", typeof(String)));
            dt.Columns.Add(new DataColumn("GMEIN", typeof(String)));
            dt.Columns.Add(new DataColumn("PLNBEZ", typeof(String)));
            dt.Columns.Add(new DataColumn("MAKTX", typeof(String)));
            dt.Columns.Add(new DataColumn("ZPLINE", typeof(String)));
            dt.Columns.Add(new DataColumn("REFDOC", typeof(String)));
            dt.Columns.Add(new DataColumn("DESC", typeof(String)));
            //GZ PID 新增
            dt.Columns.Add(new DataColumn("TTLRATE", typeof(String)));
            dt.Columns.Add(new DataColumn("TTLAMT", typeof(String)));
            return dt;
        }

        /// <summary>
        /// 存儲表身
        /// </summary>
        /// <returns></returns>
        static public DataTable BuildDetailTable()
        {
            DataTable dt = new DataTable("T_GDS_DETAIL");
            dt.Columns.Add(new DataColumn("WERKS", typeof(String)));
            dt.Columns.Add(new DataColumn("MBLNR_A", typeof(String)));
            dt.Columns.Add(new DataColumn("MJAHR_A", typeof(String)));
            dt.Columns.Add(new DataColumn("ZEILE_A", typeof(String)));
            dt.Columns.Add(new DataColumn("SOBKZ", typeof(String)));
            dt.Columns.Add(new DataColumn("MATNR", typeof(String)));
            dt.Columns.Add(new DataColumn("MENGE", typeof(double)));
            dt.Columns.Add(new DataColumn("MEINS", typeof(String)));
            dt.Columns.Add(new DataColumn("ERDAT", typeof(String)));
            dt.Columns.Add(new DataColumn("ERZEI", typeof(String)));
            dt.Columns.Add(new DataColumn("ERNAM", typeof(String)));
            dt.Columns.Add(new DataColumn("MBLNR", typeof(String)));
            dt.Columns.Add(new DataColumn("MJAHR", typeof(String)));
            dt.Columns.Add(new DataColumn("ZEILE", typeof(String)));
            dt.Columns.Add(new DataColumn("LABST", typeof(String)));
            dt.Columns.Add(new DataColumn("SLABS", typeof(String)));
            dt.Columns.Add(new DataColumn("LIFNR", typeof(String)));
            dt.Columns.Add(new DataColumn("AUFNR", typeof(String)));
            dt.Columns.Add(new DataColumn("GAMNG", typeof(String)));//AE新增 工单号码
            dt.Columns.Add(new DataColumn("VORNR", typeof(String)));
            dt.Columns.Add(new DataColumn("REFNO", typeof(String)));
            dt.Columns.Add(new DataColumn("VBELN", typeof(String)));
            dt.Columns.Add(new DataColumn("POSNR", typeof(String)));
            dt.Columns.Add(new DataColumn("KALAB", typeof(String)));
            dt.Columns.Add(new DataColumn("VBELN2", typeof(String)));
            dt.Columns.Add(new DataColumn("POSNR2", typeof(String)));
            dt.Columns.Add(new DataColumn("ERFMG", typeof(String)));
            dt.Columns.Add(new DataColumn("CHARG", typeof(String)));
            dt.Columns.Add(new DataColumn("MATN2", typeof(String)));
            dt.Columns.Add(new DataColumn("SJAHR", typeof(String)));
            dt.Columns.Add(new DataColumn("SMBLN", typeof(String)));
            dt.Columns.Add(new DataColumn("SMBLP", typeof(String)));
            dt.Columns.Add(new DataColumn("MBLNR_MSEG", typeof(String)));
            dt.Columns.Add(new DataColumn("MJAHR_MSEG", typeof(String)));
            dt.Columns.Add(new DataColumn("ZEILE_MSEG", typeof(String)));
            dt.Columns.Add(new DataColumn("POSTED", typeof(String)));
            dt.Columns.Add(new DataColumn("CHARG2", typeof(String)));
            dt.Columns.Add(new DataColumn("KUNNR", typeof(String)));
            dt.Columns.Add(new DataColumn("MBLNR_REF", typeof(String)));
            dt.Columns.Add(new DataColumn("MJAHR_REF", typeof(String)));
            dt.Columns.Add(new DataColumn("ZEILE_REF", typeof(String)));
            dt.Columns.Add(new DataColumn("PLANT_F", typeof(String)));
            dt.Columns.Add(new DataColumn("PLANT_T", typeof(String)));
            dt.Columns.Add(new DataColumn("LOC_F", typeof(String)));
            dt.Columns.Add(new DataColumn("LOC_T", typeof(String)));
            dt.Columns.Add(new DataColumn("MAKTX", typeof(String)));
            dt.Columns.Add(new DataColumn("STPRS", typeof(double)));
            dt.Columns.Add(new DataColumn("LOGGR", typeof(String)));
            dt.Columns.Add(new DataColumn("USTOCK", typeof(double)));
            dt.Columns.Add(new DataColumn("ZOVISS", typeof(String)));
            dt.Columns.Add(new DataColumn("GROES", typeof(String)));
            dt.Columns.Add(new DataColumn("RDATE", typeof(String)));
            dt.Columns.Add(new DataColumn("EKGRP", typeof(String)));
            dt.Columns.Add(new DataColumn("BESKZ", typeof(String)));
            dt.Columns.Add(new DataColumn("FEVOR", typeof(String)));
            dt.Columns.Add(new DataColumn("ZOVISS_RATE", typeof(String)));
            dt.Columns.Add(new DataColumn("OVER_AMT", typeof(double)));
            dt.Columns.Add(new DataColumn("OVER_RATE", typeof(double)));
            dt.Columns.Add(new DataColumn("GMFLOW", typeof(String)));
            dt.Columns.Add(new DataColumn("BDMNG", typeof(double)));
            dt.Columns.Add(new DataColumn("ENMNG", typeof(double)));
            dt.Columns.Add(new DataColumn("MTART", typeof(String)));
            //GZ PID 新增
            dt.Columns.Add(new DataColumn("OVER_QTY", typeof(String)));
            dt.Columns.Add(new DataColumn("DISPO", typeof(String)));
            dt.Columns.Add(new DataColumn("IPART", typeof(String)));
            //QX NA Add
            dt.Columns.Add(new DataColumn("LGPBE", typeof(String))); //架位
            dt.Columns.Add(new DataColumn("WRKST", typeof(String))); //海關品名
            dt.Columns.Add(new DataColumn("EXTWG", typeof(String))); //Ext.Mat Group
            dt.Columns.Add(new DataColumn("PMMAIL", typeof(String)));//AE 新增PM栏位
            return dt;
        }


    }
}

using System;
using System.Collections;
using System.Data;
using Liteon.ICM.DataCore;
namespace LiteOn.EA.NPIReport.Utility
{

	public class NPIMgmt
	{
        protected string _BU;
        protected string _SITE;


        public NPIMgmt(string site, string bu)
        {
            _SITE = site;
            _BU = bu;
        }

		public NPI_Standard InitialLeaveMgmt()
		{
            NPI_Standard oStandard = new NPI_Standard(_SITE, _BU);
            switch (_SITE)
			{
				case "CZ":
                    switch (_BU)
					{
						
						case "HIS":
                            oStandard = new NPI_CZ_HIS(_SITE, _BU);
							break;
						case "POWER":
                            oStandard = new NPI_CZ_POWER(_SITE, _BU);
							break;
					}
					break;
              
			}
            return oStandard;
		}	

	}
}

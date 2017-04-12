using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedCapImportConverter.Model
{
    public class AbpmModel : IModelObject
    {
        public string visit_abpm;
        public string bedtime_abp;
        public string flag_abp;
        public string awoke_abp;
        public string hours_abp;
        public string numvalall_abp;
        public string val15all_abp;
        public string ppres_all_abp;
        public string avall_sys_abp;
        public string stdall_sys_abp;
        public string avall_dia_abp;
        public string stdall_dia_abp;
        public string avall_hr_abp;
        public string stdall_hr_abp;
        public string avall_mabp_abp;
        public string stdall_mabp_abp;
        public string numvalday_abp;
        public string val15day_abp;
        public string ppres_day_abp;
        public string avday_sys_abp;
        public string stdday_sys_abp;
        public string avday_dia_abp;
        public string stdday_dia_abp;
        public string avday_hr_abp;
        public string stdday_hr_abp;
        public string avday_mabp_abp;
        public string stdday_mabp_abp;
        public string numvalnit_abp;
        public string val15nit_abp;
        public string ppres_nit_abp;
        public string avnit_sys_abp;
        public string stdnit_sys_abp;
        public string avnit_dia_abp;
        public string stdnit_dia_abp;
        public string avnit_hr_abp;

        //public string maxnit_hr_abp;
        //Need max, using std for demo
        public string stdnit_hr_abp;

        public string avnit_mabp_abp;
        public string stdnit_mabp_abp;
        public string dip_abp;
        public string bp_load_abp;
        public string bp_load_abp2;
        public decimal changesys_abp;
        public decimal changedia_abp;
        public decimal changehr_abp;

        public AbpmModel ()
        {

        }

        public void WriteToCsv()
        {
            throw new NotImplementedException();
        }
    }
}

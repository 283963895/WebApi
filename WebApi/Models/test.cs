using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Models
{

    public class test
    {
        private string _id;
        private int _Num;
        /// <summary>
        /// 编号
        /// </summary>
        [Display(Name = "编号")]
        [Required(ErrorMessage = "id 不能缺少")]
        public string Id
        {
            get
            {
                return _id;
            }

            set
            {
                _id = value;
            }
        }
        /// <summary>
        /// 数量
        /// </summary>
        [Display(Name = "数量")]
        [Required(ErrorMessage = "Num 不能缺少")]
        [Range(1, 99, ErrorMessage = "取值范围为1到99")]
        [RegularExpression(@"^[1-9]\d*$", ErrorMessage = "数量必须为1到99之间的整数")]
      
        public int Num
        {
            get
            {
                return _Num;
            }

            set
            {
                _Num = value;
            }
        }
    }
}

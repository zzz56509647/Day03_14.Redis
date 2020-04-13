using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Day03_14.Redis.Models
{
    public class Products
    {
        [Display(Name = "ID")]
        public int Id { get; set; }
        [Display(Name = "产品名称")]
        public string Pname { get; set; }
        [Display(Name = "所属品牌")]
        public string Pinpai { get; set; }
        [Display(Name = "所属分类")]
        public string Fenlei { get; set; }
        [Display(Name = "是否上架")]
        public bool SFSJ { get; set; }
        [Display(Name = "是否有货")]
        public bool SFYH { get; set; }
        [Display(Name = "更新时间")]
        public DateTime Upttime { get; set; }
    }
}
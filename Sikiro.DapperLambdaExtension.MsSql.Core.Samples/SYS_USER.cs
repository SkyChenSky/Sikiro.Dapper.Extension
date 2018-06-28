/*
 * 本文件由根据实体插件自动生成，请勿更改
 * =========================== */

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sikiro.DapperLambdaExtension.MsSql.Core.Samples
{
    [Table("SYS_USER")]
    public class SysUser
    {
        /// <summary>
        /// 主键
        /// </summary>    
        [Key]
        [Required]
        [StringLength(32)]
        [Display(Name = "主键")]
        [Column("SYS_USERID")]
        public string SysUserid { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>    
        [Required]
        [Display(Name = "创建时间")]
        [Column("CREATE_DATETIME")]
        public DateTime CreateDatetime { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>    
        [Required]
        [StringLength(32)]
        [Display(Name = "邮箱")]
        [Column("EMAIL")]
        public string Email { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>    
        [Required]
        [StringLength(11)]
        [Display(Name = "手机号")]
        [Column("MOBILE")]
        public string Mobile { get; set; }

        /// <summary>
        /// 密码
        /// </summary>    
        [Required]
        [StringLength(32)]
        [Display(Name = "密码")]
        [Column("PASSWORD")]
        public string Password { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>    
        [Required]
        [StringLength(16)]
        [Display(Name = "姓名")]
        [Column("REAL_NAME")]
        public string RealName { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>    
        [Required]
        [StringLength(16)]
        [Display(Name = "用户名")]
        [Column("USER_NAME")]
        public string UserName { get; set; }

        /// <summary>
        /// USER_STATUS
        /// </summary>    
        [Required]
        [Display(Name = "USER_STATUS")]
        [Column("USER_STATUS")]
        public int UserStatus { get; set; }

        /// <summary>
        /// USER_TYPE
        /// </summary>    
        [Required]
        [Display(Name = "USER_TYPE")]
        [Column("USER_TYPE")]
        public int UserType { get; set; }
    }
}

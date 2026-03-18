using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Blog.Core.Commons;
using SqlSugar;

namespace Blog.Core.Entities
{
    [SugarTable("blog_scheduled_task")]

    public partial class BlogScheduledTask
    {
        /// <summary>
        /// 主键（应用生成的 long）
        /// </summary>
        [Key]
        [SugarColumn(ColumnName = "blog_scheduled_task_id", IsPrimaryKey = true)]
        public long BlogScheduledTaskId { get; set; }

        /// <summary>
        /// 任务名称
        /// </summary>
        [MaxLength(200)]
        [Required]
        [SugarColumn(ColumnName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Cron 表达式（调度）
        /// </summary>
        [MaxLength(200)]
        [SugarColumn(ColumnName = "cron_expression")]
        public string CronExpression { get; set; }

        /// <summary>
        /// 任务负载（JSON）
        /// </summary>
        [SugarColumn(ColumnName = "payload")]
        public string Payload { get; set; }

        /// <summary>
        /// 下一次执行时间
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        [JsonConverter(typeof(JsonNullableDateTimeConverter))]
        [SugarColumn(ColumnName = "next_run_at")]
        public DateTime? NextRunAt { get; set; }

        /// <summary>
        /// 最后一次执行时间
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        [JsonConverter(typeof(JsonNullableDateTimeConverter))]
        [SugarColumn(ColumnName = "last_run_at")]
        public DateTime? LastRunAt { get; set; }

        /// <summary>
        /// 是否启用：1=启用，0=禁用
        /// </summary>
        [SugarColumn(ColumnName = "enabled")]
        public int Enabled { get; set; }

        /// <summary>
        /// 记录创建时间
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        [JsonConverter(typeof(JsonDateTimeConverter))]
        [SugarColumn(ColumnName = "create_at", InsertSql = "CURRENT_TIMESTAMP")]
        public DateTime CreateAt { get; set; }

        /// <summary>
        /// 记录创建者（用户 id）
        /// </summary>
        [SugarColumn(ColumnName = "create_by")]
        public long? CreateBy { get; set; }

        /// <summary>
        /// 记录更新时间
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        [JsonConverter(typeof(JsonDateTimeConverter))]
        [SugarColumn(ColumnName = "update_at", InsertSql = "CURRENT_TIMESTAMP", UpdateSql = "CURRENT_TIMESTAMP")]
        public DateTime UpdateAt { get; set; }

        /// <summary>
        /// 记录更新者（用户 id）
        /// </summary>
        [SugarColumn(ColumnName = "update_by")]
        public long? UpdateBy { get; set; }

    }
}

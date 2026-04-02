using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.FileStorage.Core
{
    public class FileUploadContext
    {
        private readonly Dictionary<string, FileUploadStrategy> _strategies;
        private readonly string _defaultStrategyName;

        // 构造函数注入所有实现类
        public FileUploadContext(IEnumerable<FileUploadStrategy> strategies, string defaultStrategy = "Local")
        {
            _strategies = strategies.ToDictionary(s => s.GetType().Name, s => s);
            _defaultStrategyName = defaultStrategy;
        }
        // 获取指定策略，如果未指定则使用默认策略
        public FileUploadStrategy GetStrategy(string strategyName = null)
        {
            var name = strategyName ?? _defaultStrategyName;

            if (_strategies.TryGetValue(name, out var strategy))
            {
                return strategy;
            }

            throw new InvalidOperationException($"未找到名为 '{name}' 的上传策略");
        }

    }
}

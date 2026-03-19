// 扩展 jQuery 的 serializeObject 方法
$.fn.serializeObject = function () {
    // 初始化空对象存储结果
    var obj = {};
    // 获取表单序列化后的数组（每个元素包含 name 和 value）
    var arr = this.serializeArray();

    // 遍历数组，组装成 JSON 对象
    $.each(arr, function () {
        // 如果同一个 name 有多个值（如多选框），则转为数组
        if (obj[this.name] !== undefined) {
            if (!obj[this.name].push) {
                obj[this.name] = [obj[this.name]];
            }
            obj[this.name].push(this.value || '');
        } else {
            obj[this.name] = this.value || '';
        }
    });

    return obj;
};
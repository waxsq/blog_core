/**
 * 将 serializeObject 的结果转换为后端查询数组格式
 * @param {Object} formObj - serializeObject() 返回的对象 { key: value }
 * @param {string} defaultCondition - 默认条件类型，默认 "0" (等于)
 * @returns {Array} [{ FieldName, ConditionalType, FieldValue }, ...]
 */
function convertToQueryArray(formObj, defaultCondition = "0") {
    if (!formObj || typeof formObj !== 'object') {
        return [];
    }

    return Object.keys(formObj)
        .filter(key => {
            // 1. 过滤掉空值、null、undefined
            const val = formObj[key];
            return val !== null && val !== undefined && val.toString().trim() !== '';
        })
        .map(key => ({
            FieldName: key,                // 原对象的 key
            ConditionalType: defaultCondition, // 默认条件
            FieldValue: formObj[key].toString().trim() // 原对象的 value
        }));
}

/**
 * 通用表单填充函数
 * @param {Object} data - 数据对象
 * @param {String|HTMLElement} formSelector - 表单选择器或DOM对象
 */
let fillForm = function (data, formSelector) {
    const form = typeof formSelector === 'string' ? document.querySelector(formSelector) : formSelector;
    if (!form) return;

    // 遍历表单内的所有控件
    Array.from(form.elements).forEach(el => {
        const name = el.name;
        if (!name || !(name in data)) return;

        const value = data[name];

        // 处理不同类型的控件
        switch (el.type) {
            case 'checkbox':
                el.checked = Array.isArray(value) ? value.includes(el.value) : !!value;
                break;
            case 'radio':
                el.checked = el.value === value;
                break;
            case 'select-one':
            case 'select-multiple':
                // select 需要特殊处理多选，这里简化处理
                el.value = value;
                break;
            default:
                el.value = value !== null && value !== undefined ? value : '';
        }
    });
}


let sendAjax = function (url, data = {}, success, error, complete, type = "POST", dataType = 'json', contentType = 'application/json', headers = {},) {
    if (contentType === 'application/json' && typeof data === 'object' && data !== null) {
        data = JSON.stringify(data);
    }
    $.ajax({
        url,          // 请求地址
        type,                    // 请求方式：'GET', 'POST', 'PUT', 'DELETE' 等
        data,
        dataType,               // 预期服务器返回的数据类型：'json', 'xml', 'html', 'text'
        contentType, // 发送数据的格式 (默认是 'application/x-www-form-urlencoded')
        timeout: 5000,                  // 超时时间 (毫秒)
        headers,
        // --- 回调函数 ---
        // 请求成功时调用 (注意：只有当 dataType 匹配且 HTTP 状态码为 2xx 时才触发)
        success,
        // 请求失败时调用 (网络错误、超时、HTTP 4xx/5xx、JSON 解析失败)
        error,
        // 请求完成时调用 (无论成功失败都会执行，常用于关闭 loading 遮罩)
        complete
    });
}
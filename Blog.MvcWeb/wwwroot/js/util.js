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
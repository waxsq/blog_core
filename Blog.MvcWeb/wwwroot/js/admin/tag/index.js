layui.use(['form', 'table', 'layer'], function () {
    var form = layui.form;
    var table = layui.table;
    var layer = layui.layer;


    table.render({
        elem: '#ID-table-demo-data',
        url: '/Admin/Tag/Query',
        page: true,
        cols: [[
            { field: 'blogTagId', title: '主键', hide: true },
            { type: 'checkbox', fixed: 'left' },
            { field: 'tagName', title: '标签名称' },
            { field: 'isValid', title: '状态', templet: '#isValidTpl' },
            { fixed: 'right', title: '操作', align: 'center', toolbar: '#rowToolbarTpl' }
        ]],
        where: {},
        request: {
            pageName: 'PageIndex',
            limitName: 'PageSize'
        },
        toolbar: "#toolbarTpl",
        parseData: function (res) {
            // 如果你的后端返回格式特殊，可以在这里统一转换
            // 假设后端返回: { code: 200, data: { list: [...], total: 100 } }
            if (res.code !== 200) {
                return { code: 1, msg: res.message, count: 0, data: [] };
            }
            // 适配你的 ResultReponse<PageModel<T>> 结构
            // 假设 res.data 就是 PageModel 对象
            return {
                code: 0, // Layui 要求成功为 0
                msg: res.message || 'success',
                count: res.data.totalCount, // 总条数
                data: res.data.items        // 数据列表
            };
        }
    })

    form.on('submit(search-form)', function (data) {
        var fieldData = data.field; // 获取表单所有字段 { id: "1", name: "jack" ... }

        // 调用你之前的转换方法，变成 [{FieldName, ConditionalType, FieldValue}]
        // 注意：这里假设 convertToQueryArray 是你全局定义的方法
        var queryArray = window.convertToQueryArray(fieldData, "0");

        // 3. 核心：使用 table.reload 重载表格
        table.reload('ID-table-demo-data', { // 注意这里填的是 elem 的 ID (去掉#)
            where: {
                params: JSON.stringify(queryArray)
            },
            page: {
                curr: 1 // 搜索时重置到第 1 页
            },
            method: 'post' // 建议改为 POST，因为查询条件数组可能很长
        });

        return false; // 阻止表单默认跳转
    });


    table.on('tool(ID-table-demo-data)', function (obj) {
        var data = obj.data;
        var event = obj.event;
        switch (event) {
            case 'Edit':
                openDialog(data.blogTagId, 'edit')
                break;
            case 'Del':
                break;
        }
    })

    table.on('toolbar(ID-table-demo-data)', function (obj) {
        var options = obj.config;
        var checkStatus = table.checkStatus(options.id);
        switch (obj.event) {
            case 'Add':
                openDialog(0, 'Add')
                break;
        }
    })

    let openDialog = function (id, action = 'view') {
        layer.open({
            type: 2,
            content: `/Admin/Tag/AddOrEdit?id=${id}&action=${action}`, //这里content是一个URL，如果你不想让iframe出现滚动条，你还可以content: ['http://sentsin.com', 'no']
            area: ['600px', '400px'],
            btn: ['确定', '取消'],
            yes: function (index, layero) {
                var iframeWin = window[layero.find('iframe')[0]['name']];
                var addOrEditFormObj = iframeWin.$('#addOrEditForm');
                var data = addOrEditFormObj.serializeObject();
                if (action != 'view') {
                    debugger
                    sendAjax(`/Admin/Tag/${action}`, data,
                        function (res) {
                            if (res.success) {
                                layer.msg('操作成功', { icon: 1, time: 2000 });
                            } else {
                                layer.msg(`操作失败:${res.message}`, { icon: 2, time: 4000 });
                            }
                        }, function (error) {
                            layer.msg('操作失败', { icon: 2, time: 4000 })
                        }, function () {
                            layer.close(index)
                        })
                }
            },
            btn1: function (index) {

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

})
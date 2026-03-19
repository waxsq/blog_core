layui.use(['form', 'table', 'layer'], function () {
    var form = layui.form;
    var table = layui.table;
    var layer = layui.layer;


    table.render({
        elem: '#ID-table-demo-data',
        url: '',
        page: true,
        cols: [[
            { field: 'blogTagId', title:'主键', hide: true },
            { type: 'checkbox', fixed: 'left' },
            { field: 'blogTagCode', title: '标签编码' },
            { field: 'blogTagName', title: '标签名称' },
            { field: 'isValid', title: '状态', templet: '#isValidTpl' },
            { fixed: 'right', title: '操作', align: 'center', toolbar: '#rowToolbarTpl' }
        ]],
        toolbar: "#toolbarTpl"
    })


    table.on('tool(ID-table-demo-data)', function (obj) {
        var data = obj.data;
        var event = obj.event;
        switch (event) {
            case 'add':
                break;
            case 'edit':
                openDialog(data.blogTagId, 'edit')
                break;
            case 'del':
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
                sendAjax('/')
                layer.close(index)
            },
            btn1: function (index) {

            }
        });
    }



    let sendAjax = function (url, type, data = {}, dataType = 'json', contentType = 'application/json', headers = {}, success, error, complete) {
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
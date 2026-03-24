layui.use(['form', 'table', 'layer'], function () {
    var form = layui.form;
    var table = layui.table;
    var layer = layui.layer;
    var $ = layui.$;

    var currentTable = table.render({
        elem: '#ID-table-demo-data',
        url: '/Tag/QueryPage',
        contentType: 'application/json', // 👈 关键：告诉服务器我发的是 JSON
        page: true,
        cols: [[
            { field: 'blogTagId', title: '主键', hide: true },
            { type: 'checkbox', fixed: 'left' },
            { field: 'tagName', title: '标签名称', templet :'#viewDetailTpl' },
            { field: 'isValid', title: '状态', templet: '#isValidTpl' },
            { fixed: 'right', title: '操作', align: 'center', toolbar: '#rowToolbarTpl' }
        ]],
        method: 'post',
        where: {
            PageIndex: 1,
            PageSize: 10,
            ...($("#search-form").serializeObject())
        },
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
                count: res.totalCount, // 总条数
                data: res.datas        // 数据列表
            };
        }
    })

    form.on('submit(submit-form)', function (data) {
        // 3. 核心：使用 table.reload 重载表格
        table.reload('ID-table-demo-data', { // 注意这里填的是 elem 的 ID (去掉#)
            where: {
                ...data.field
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
                openDialog(data.blogTagId, event)
                break;
            case 'Del':
                layer.confirm(`是否删除该数据:标签${data.tagName}?`, { icon: 3, title: 'tips' }, function (index) {
                    sendAjax("/Tag/DeleteById", { blogTagId: data.blogTagId }, function (res) {
                        if (res.success && res.code == 200) {
                            layer.msg('操作成功', { icon: 1, time: 2000 });
                        } else {
                            layer.msg(`操作失败:${res.message}`, { icon: 2, time: 4000 });
                        }
                    }, function (error) { }, function () { layui.close(index) })
                });
                break;
            case 'View':
                openDialog(data.blogTagId, event)
                break;
        }
    })

    table.on('toolbar(ID-table-demo-data)', function (obj) {
        var options = obj.config;
        var checkStatus = table.checkStatus(options.id);
        switch (obj.event) {
            case 'Add':
                openDialog(0, obj.event)
                break;
        }
    })


    let openDialog = function (id, action = 'View') {
        layer.open({
            type: 2,
            content: `/Admin/Tag/AddOrEdit?id=${id}&action=${action}`, //这里content是一个URL，如果你不想让iframe出现滚动条，你还可以content: ['http://sentsin.com', 'no']
            area: ['600px', '400px'],
            btn: ['确定', '取消'],
            yes: function (index, layero) {
                var iframeWin = window[layero.find('iframe')[0]['name']];
                var addOrEditFormObj = iframeWin.$('#addOrEditForm');
                var data = addOrEditFormObj.serializeObject();
                if (action != 'View') {
                    sendAjax(`/Tag/${action}`, data,
                        function (res) {
                            if (res.success) {
                                layer.msg('操作成功', { icon: 1, time: 2000 });
                            } else {
                                layer.msg(`操作失败:${res.message}`, { icon: 2, time: 4000 });
                            }
                        }, function (error) {
                            layer.msg('操作失败', { icon: 2, time: 4000 })
                        }, function () {
                            currentTable.reload({
                                page: {
                                    curr: 1
                                },
                                where: form.val('search-form')
                            })
                            layer.close(index)
                        })
                }
            },
            btn1: function (index) {

            }
        });
    }
})
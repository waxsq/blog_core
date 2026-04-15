layui.use(['form', 'table', 'layer', 'laydate', 'util'], function () {
    var form = layui.form;
    var table = layui.table;
    var $ = layui.$;
    var laydate = layui.laydate;
    var layer = layui.layer;
    var util = layui.util;

    //初始化时间组件
    laydate.render({
        elem: '#createBeginAt',
        type: 'datetime',
        format: 'yyyy-MM-dd HH:mm:ss',
        done: function (value, beginDate, endDate) {
            var endDate = $('#createEndAt').val();
            if (endDate && value && (endDate < value)) {
                layer.msg('结束时间不能早于开始时间，请重新选择！', {
                    icon: 2,      // 【关键】图标：2 代表哭脸/错误/异常 (0-6都有不同含义)
                    time: 3000,   // 停留时间：3000毫秒 (3秒)，默认是3000，可省略
                    shift: 6,     // 动画：6 代表抖动动画 (强调错误)
                    shade: 0.1    // 遮罩透明度：0.1 (轻微遮罩，让用户注意到但不过分干扰)
                });
                setTimeout(() => $('#createBeginAt').val(''), 50)
                return;
            }

        }
    })
    laydate.render({
        elem: '#createEndAt',
        type: 'datetime',
        format: 'yyyy-MM-dd HH:mm:ss',
        done: function (value, beginDate, endDate) {
            var beginDate = $('#createBeginAt').val();
            if (value && beginDate && (value < beginDate)) {
                layer.msg('结束时间不能早于开始时间，请重新选择！', {
                    icon: 2,      // 【关键】图标：2 代表哭脸/错误/异常 (0-6都有不同含义)
                    time: 3000,   // 停留时间：3000毫秒 (3秒)，默认是3000，可省略
                    shift: 6,     // 动画：6 代表抖动动画 (强调错误)
                    shade: 0.1    // 遮罩透明度：0.1 (轻微遮罩，让用户注意到但不过分干扰)
                });
                setTimeout(() => $('#createEndAt').val(''), 50)
                return;
            }
        }
    })

    //渲染表格
    var currentTable = table.render({
        elem: '#ID-table-demo-data',
        url: '/Comment/QueryPage',
        contentType: 'application/json', // 👈 关键：告诉服务器我发的是 JSON
        page: true,
        cols: [[
            { field: 'blogCommentId', title: '主键', hide: true },
            { type: 'checkbox', fixed: 'left' },
            { field: 'postTitle', title: '文章标题'},
            { field: 'content', title: '评论' },
            { field: 'status', title: '发布状态', templet: '#statusTpl' },
            { field: 'likesCount', title: '点赞人数' },
            { field: 'userName', title: '评论人' },
            { field: 'createAt', title: '发布时间' },
            { field: 'address', title: 'IP地址' },
            { fixed: 'right', title: '操作', templet: '#rowToolbarTpl' }
        ]],
        toolbar: '#toolbarTpl',
        method: 'post',
        where: {
            ...($("#search-form").serializeObject())
        },
        request: {
            pageName: 'PageIndex',
            limitName: 'PageSize'
        },
        parseData: function (res) {
            if (res.code !== 200) {
                return { code: 1, msg: res.message, count: 0, data: [] };
            }
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

    form.on('submit(reset-form)', function (data) {
        // 1. 清空表单输入框
        data.form.reset(); // data.form 是原生的 form DOM 对象

        refresh()

        return false; // 阻止表单跳转
    });

    //单元格工具栏
    table.on('tool(ID-table-demo-data)', function (obj) {
        var data = obj.data;
        var event = obj.event;
        openSureDialog(data, event);
    })

    //表头工具栏
    table.on('toolbar(ID-table-demo-data)', function (obj) {
        var options = obj.config;
        var checkStatus = table.checkStatus(options.id);
        var checkData = checkStatus.data;
        openSureDialog(checkData, obj.event)
    })

    let refresh = function () {
        currentTable.reload({
            page: {
                curr: 1
            },
            where: form.val('search-form')
        })
    }

    let openSureDialog = function (data,status) {
        var ids = [];
        if (Array.isArray(data)) {
            ids = data.map(item => item.blogCommentId);
        } else {
            ids.push(data.blogCommentId);
        }
        var message = status == 1 ? "通过审核" : "屏蔽";
        layer.confirm(`是否进行${message}操作`, {
            icon: 3,
            title:'tips'
        }, function (index) {
            sendAjax("Comment/UpdateStatus", {
                ids,
                status
            }, function (res) {
                if (res.code != 200) {
                    layer.msg(res.message, {
                        icon: 2,
                        time: 3000,
                        shift: 6,
                        shade: 0.1
                    });
                }
            }, function (error) {
                layer.msg(error.statusText, {
                    icon: 2,
                    time: 3000,
                });
            }, function () {
                layer.close(index)
            })
        })
    }
})
layui.use(['form', 'table', 'layer', 'laydate', 'util'], function () {
    var form = layui.form;
    var table = layui.table;
    var $ = layui.$;
    var laydate = layui.laydate;
    var layer = layui.layer;
    var util = layui.util;




    //初始化时间组件
    laydate.render({
        elem: '#publishedBeginAt',
        type: 'datetime',
        format: 'yyyy-MM-dd HH:mm:ss',
        done: function (value, beginDate, endDate) {
            var endDate = $('#publishedEndAt').val();
            if (endDate && value && (endDate < value)) {
                layer.msg('结束时间不能早于开始时间，请重新选择！', {
                    icon: 2,      // 【关键】图标：2 代表哭脸/错误/异常 (0-6都有不同含义)
                    time: 3000,   // 停留时间：3000毫秒 (3秒)，默认是3000，可省略
                    shift: 6,     // 动画：6 代表抖动动画 (强调错误)
                    shade: 0.1    // 遮罩透明度：0.1 (轻微遮罩，让用户注意到但不过分干扰)
                });
                setTimeout(() => $('#publishedBeginAt').val(''), 50)
                return;
            }

        }
    })
    laydate.render({
        elem: '#publishedEndAt',
        type: 'datetime',
        format: 'yyyy-MM-dd HH:mm:ss',
        done: function (value, beginDate, endDate) {
            var beginDate = $('#publishedBeginAt').val();
            if (value && beginDate && (value < beginDate)) {
                layer.msg('结束时间不能早于开始时间，请重新选择！', {
                    icon: 2,      // 【关键】图标：2 代表哭脸/错误/异常 (0-6都有不同含义)
                    time: 3000,   // 停留时间：3000毫秒 (3秒)，默认是3000，可省略
                    shift: 6,     // 动画：6 代表抖动动画 (强调错误)
                    shade: 0.1    // 遮罩透明度：0.1 (轻微遮罩，让用户注意到但不过分干扰)
                });
                setTimeout(() => $('#publishedEndAt').val(''), 50)
                return;
            }
        }
    })

    //渲染表格
    var currentTable = table.render({
        elem: '#ID-table-demo-data',
        url: '/Post/QueryPage',
        contentType: 'application/json', // 👈 关键：告诉服务器我发的是 JSON
        page: true,
        cols: [[
            { field: 'blogPostId', title: '主键', hide: true },
            { type: 'checkbox', fixed: 'left' },
            { field: 'title', title: '文章标题', templet: '#viewDetailTpl' },
            { field: 'summary', title: '文章简介' },
            { field: 'status', title: '发布状态', templet: '#statusTpl' },
            { field: 'isFeatured', title: '是否推荐', templet: '#isFeaturedTpl' },
            { field: 'isTop', title: '是否顶置', templet: '#isTopTpl' },
            { field: 'categoryId', hide: true },
            { field: 'categoryName', title: '分类' },
            { field: 'viewsCount', title: '查看人数' },
            { field: 'commentsCount', title: '评论人数' },
            { field: 'likesCount', title: '点赞人数' },
            { field: 'updateAt', title: '最近修改时间' },
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



    table.on('tool(ID-table-demo-data)', function (obj) {
        var data = obj.data;
        var event = obj.event;
        switch (event) {
            case 'Edit':
                openDialog(data.blogPostId, event)
                break;
            case 'Del':
                layer.confirm(`是否删除该数据:文章${data.title}?`, { icon: 3, title: 'tips' }, function (index) {
                    sendAjax("/Post/DeleteById", { blogPostId: data.blogPostId }, function (res) {
                        if (res.success && res.code == 200) {
                            layer.msg('操作成功', { icon: 1, time: 2000 });
                            currentTable.reload({
                                page: {
                                    curr: 1
                                },
                                where: form.val('search-form')
                            })
                        } else {
                            layer.msg(`操作失败:${res.message}`, { icon: 2, time: 4000 });
                        }
                    }, function (error) {
                        layer.msg(`操作失败:${error.statusText}`, { icon: 2, time: 4000 });
                    }, function () { layer.close(index) })
                });
                break;
            case 'View':
                openDialog(data.blogPostId, event)
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

        var btn = action == "View" ? [] : ['确定', '取消'];

        var fullIndex = layer.open({
            title: '文章',
            maxmin: true,
            type: 2,
            scrollbar: true,
            fixed: true,
            resize: true,
            content: `/Admin/Post/AddOrEdit?id=${id}&action=${action}`, //这里content是一个URL，如果你不想让iframe出现滚动条，你还可以content: ['http://sentsin.com', 'no']
            btn: btn,
            yes: function (index, layero) {
                var iframe = layero.find('iframe')[0];
                var childWindow = iframe.contentWindow;
                if (!childWindow.validate()) {
                    return;
                }
                var data = childWindow.getAddOrEditData();
                if (action != 'View') {
                    sendAjax(`/Post/${action}`, data,
                        function (res) {
                            if (res.success) {
                                layer.msg('操作成功', { icon: 1, time: 2000 });
                                currentTable.reload({
                                    page: {
                                        curr: 1
                                    },
                                    where: form.val('search-form')
                                })
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
        layer.full(fullIndex);
    }

})
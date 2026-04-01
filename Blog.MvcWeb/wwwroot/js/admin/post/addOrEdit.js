layui.use(['form', 'layer', 'tableSelect'], function () {
    var form = layui.form;
    var layer = layui.layer;
    var tableSelect = layui.tableSelect;
    var $ = layui.$;
    var contentEditor;
    //tag的id
    let tagSelected = [];

    //初始化
    $(function () {
        contentEditor = editormd('content-editormd', {
            width: "100%",
            height: "80vh",
            syncScrolling: "single",
            path: "/lib/editor.md/lib/",
            imageUpload: true,
            imageFormats: ["jpg", "jpeg", "gif", "png", "bmp", "webp"],
            imageUploadURL:''
        })
    })

    //下拉表格
    tableSelect.render({
        elem: '#categorySelect',
        checkedKey: 'blogCategoryId',
        searchKey: 'categoryName',
        searchPlaceholder: '关键词搜索',
        table: {
            url: '/Category/QueryPage',
            contentType: 'application/json', // 👈 关键：告诉服务器我发的是 JSON
            page: true,
            cols: [[
                { field: 'blogCategoryId', title: '主键', hide: true },
                { type: 'radio', fixed: 'left' },
                { field: 'categoryName', title: '分类名称' },
            ]],
            method: 'post',
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
        },
        done: function (elem, data) {
            if (data && data.data.length > 0) {
                var choseData = data.data[0];
                fillForm(choseData, '#addOrEdit');
            }
        }
    });


    xmSelect.render({
        el: '#tagSelect',
        checkbox: true,
        paging: true,
        pageSize: 10,
        pageRemote: true,
        prop: {
            name: 'tagName',
            value: 'blogTagId'
        },
        initValue: tagSelected,
        remoteMethod: function (keywords, cb, isSearch, page) {
            sendAjax(
                '/Tag/QueryPage',  // 第1个参数：URL 字符串
                {                  // 第2个参数：Data 对象
                    tagName: keywords,
                    pageIndex: page,
                    pageSize: 10,
                },
                function (res) {    // 第3个参数：Success 回调函数
                    if (res.code === 200) {
                        cb(res.datas, res.totalCount);
                    } else {
                        cb([], 0);
                    }
                }
            );
        },
        on: function (data) {
            var arr = data.arr;
            console.log(arr);
        }
    })

});
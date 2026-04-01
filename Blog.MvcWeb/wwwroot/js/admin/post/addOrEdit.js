layui.use(['form', 'layer', 'tableSelect', 'selectM'], function () {
    var form = layui.form;
    var layer = layui.layer;
    var tableSelect = layui.tableSelect;
    var selectM = layui.selectM;
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
            path: "/lib/editor.md/lib/"
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
                { field: 'categoryName', title: '分类名称', templet: '#viewDetailTpl' },
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
            //选择完后的回调，包含2个返回值 elem:返回之前input对象；data:表格返回的选中的数据 []
            //拿到data[]后 就按照业务需求做想做的事情啦~比如加个隐藏域放ID...
            if (data && data.data.length > 0) {
                var choseData = data.data[0];
                fillForm(choseData, '#addOrEdit');
            }
        }
    });

    selectM({
        elem: '#tagSelect',
        data: '/Tag/QueryPage',
        selected: [],
        max: 5,
        field: {
            idName: 'blogTagId',
            titleName: 'tagName',
            statusName: 'isValid'
        },
        name: 'tagName',
    })

});
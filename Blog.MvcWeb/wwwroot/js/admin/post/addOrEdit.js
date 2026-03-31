layui.use(['form', 'layer', 'tableSelect'], function () {
    var form = layui.form;
    var layer = layui.layer;
    var tableSelect = layui.tableSelect;
    var $ = layui.$;
    var contentEditor;
    //初始化
    $(function () {
        contentEditor = editormd('content-editormd', {
            width: "100%",
            height: "80vh",
            syncScrolling: "single",
            path: "/lib/editor.md/lib/"
        })
    })

    
});
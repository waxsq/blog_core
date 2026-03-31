layui.use(['form', 'layer'], function () {
    var form = layui.form;
    var layer = layui.layer;
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
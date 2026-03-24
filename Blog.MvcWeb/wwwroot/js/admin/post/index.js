layui.use(['form', 'table', 'layer','laydate'], function () {
    var form = layui.form;
    var table = layui.table;
    var $ = layui.$;
    var laydate = layui.laydate;


    //初始化时间组件
    laydate.render({
        elem: '#publishedAt',
        type: 'datetime',
        range: true 
    })

    //渲染表格
    table.render({
        elem: '#ID-table-demo-data',
        url: '/Post/QueryPage',
        contentType: 'application/json', // 👈 关键：告诉服务器我发的是 JSON
        page: true,

    })

})
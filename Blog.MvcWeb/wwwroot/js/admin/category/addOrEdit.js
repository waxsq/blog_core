layui.use(['form', 'layer'], function () {
    var form = layui.form;
    var layer = layui.layer;
    var $ = layui.$;

    //初始化
    $(function () {
        if (action != 'Add') {
            var index = layer.load(2);
            sendAjax('/Category/GetById', { blogCategoryId: id , action }, function (res) {
                if (res.success && res.code == 200) {
                    var data = res.data;
                    form.val('addOrEditForm', data);
                } else {
                    layer.msg(`操作失败:${res.message}`, { icon: 2, time: 4000 });
                }
            }, function (error) {

            }, function () {
                layer.close(index);
            })
        }
        if (action == 'View') {
            $('#addOrEditForm').find('input,select,button').prop('disabled', true);
        }
    })
});

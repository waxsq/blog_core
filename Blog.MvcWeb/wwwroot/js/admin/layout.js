layui.config({
    base: '/lib/layuiadmin/' // 假设layuiadmin放在这个目录
}).extend({
    index: '/lib/index'       // 主入口模块
}).use(['index','element'], function () {
    // 初始化完成后可以执行自定义代码
    console.log('LayuiAdmin 初始化完成');
});
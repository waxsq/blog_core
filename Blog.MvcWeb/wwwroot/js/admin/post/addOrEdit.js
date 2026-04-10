layui.use(['form', 'layer', 'tableSelect'], function () {
    var form = layui.form;
    var layer = layui.layer;
    var tableSelect = layui.tableSelect;
    var $ = layui.$;
    var tagSelected = [];
    var contentEditor;

    //自定义验证规则
    form.verify({
        title: function (value, item) {
            if (!value) return "标题为必填";
            if (value.length < 2 || value.length > 100) return "标题长度必须在2-100之间";
        },
        summary: function (value, item) {
            if (!value) return "简介为必填";
            if (value.length < 10 || value.length > 500) return "简介长度必须在10-500之间";
        },
        content: function (value, item) {
            if (!value) return "内容为必填";
        }
    });

    tableSelectDom = tableSelect.render({
        elem: '#categorySelect',
        checkedKey: 'blogCategoryId',
        searchKey: 'categoryName',
        searchPlaceholder: '关键词搜索',
        table: {
            url: '/Category/QueryPage',
            contentType: 'application/json', // 👈 关键：告诉服务器我发的是 JSON
            page: true,
            cols: [[
                { type: 'radio', fixed: 'left' },
                { field: 'blogCategoryId', hide: true },
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
                form.val('addOrEdit', choseData)
                $('#categoryId').val(choseData.blogCategoryId);
            }
        }
    });


    contentEditor = editormd('content-editormd', {
        syncScrolling: "single",
        path: "/lib/editor.md/lib/",
        imageUpload: true,
        imagePaste: true,
        readOnly: action == "View",
        imageFormats: ["jpg", "jpeg", "gif", "png", "bmp", "webp"],
        imageUploadURL: "/File/Img",
        onload: function () {
            //处理图片复制粘贴上传
            initPasteDragImg(this);
        },
        onfullscreen: function () {
            //打开全屏
            $(".full-hiden").hide();
        },
        onfullscreenExit: function () {
            //退出全屏
            $(".full-hiden").show();
            // 【关键代码】强制重置宽度和高度
            var that = this;
            setTimeout(function () {
                // 重置父容器宽度
                that.container.css({
                    "width": "100%",
                    "height": "80vh"
                });
                // 重置编辑器内部宽度
                that.editor.css({
                    "width": "100%",
                    "height": "80vh"
                });

                // 如果还有预览区域，也需要重置
                if (that.preview) {
                    that.preview.css("height", "80vh");
                }

                // 触发窗口resize事件，让editor.md重新计算布局（可选）
                $(window).trigger('resize');
            }, 10);
        }
    })


    function initTagSelect() {
        xmSelect.render({
            el: '#tagSelect',
            checkbox: true,
            name: 'tagIds',
            paging: true,
            pageSize: 10,
            pageRemote: true,
            disabled: action == "View",
            layVerify: 'required',  // 验证规则
            layVerType: 'tips',     // 验证提示方式
            layReqText: '请选择标签', // 必填提示文字
            tips: '请选择标签',       // 选择框默认提示文字
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
    }


    //初始化
    $(function () {
        if (action != 'Add') {
            if (action == 'View') {
                $("#addOrEdit input, #addOrEdit select,#addOrEdit textarea").prop("disabled", true);
            }
            var loadIndex = layer.load(0);
            //获取数据
            sendAjax("/Post/GetById", { blogPostId: id, action }, function (result) {
                if (result.code == 200) {
                    var data = result.data;
                    tagSelected = data?.tags.map(item => {
                        return {
                            blogTagId: item.blogTagId,
                            tagName: item.tagName
                        }
                    });
                    initTagSelect();
                    $('#categorySelect').attr("ts-selected", data.categoryId);
                    $('#categorySelect').val(data.categoryName);
                    contentEditor.setMarkdown(data.content)
                    form.val('addOrEdit', data);

                } else {
                    layer.msg(`操作失败:${result.message}`, { icon: 2, time: 4000 });
                }
            }, function (error) {
                layer.msg(`操作失败:${error}`, { icon: 2, time: 4000 });
            }, function () {
                layer.close(loadIndex);
            })
        } else {
            initTagSelect()
        }

    })


    window.validate = function () {
        return form.validate("#addOrEdit");
    }

    //返回数据
    window.getAddOrEditData = function () {
        return form.val('addOrEdit');
    }
});

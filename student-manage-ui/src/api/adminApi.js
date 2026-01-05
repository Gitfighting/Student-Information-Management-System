import request from './request'
/**
 * 管理员登录接口（对接BLL层CheckLogin方法）
 * @param {Object} data - 登录参数（adminName:用户名, adminPwd:密码）
 */
export const adminLogin = (data) => {

	console.log('管理员登录请求参数:', data);
	return request({
		url: '/admin/checkLogin', // 后端接口地址（需与API层控制器路由一致）
		method: 'post',
		data
	})
}
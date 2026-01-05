import request from './request'

/**
 * 学生登录接口（对接BLL层CheckLogin方法）
 * @param {Object} data - 登录参数（stuName:用户名, stuPwd:密码）
 */
export const studentLogin = (data) => {

	console.log('学生登录请求参数:', data);
	return request({
		url: '/student/checkLogin', // 后端接口地址（需与API层控制器路由一致）
		method: 'post',
		data
	})
}

/**
 * 学生注册接口（对接BLL层Register方法）
 * @param {Object} data - 注册参数（stuId/name/gender等，与Student实体一致）
 */
export const studentRegister = (data) => {
	return request({
		url: '/student/register', // 改为后端路由：api/student/register
		method: 'post',
		data // 数据需与Model层Student.cs的属性名一致（如stuId、stuName、stuAge等）
	})
}

/**
 * 查看个人信息接口（对接BLL层QueryPersonalInfo方法）
 * @param {Number} stuId - 学生学号（登录后从Pinia获取）
 */
export const getStudentProfile = (stuId) => {
	return request({
		url: `/student/queryPersonalInfo?stuId=${stuId}`, // 路径参数传递学号
		method: 'get'
	})
}

/**
 * 获取所有学生
 */
export const getAllStudents = () => {
	return request({
		url: '/student/getAll',
		method: 'get'
	})
}

/**
 * 多条件查询学生（管理员）
 * @param {Object} data - 查询条件
 */
export const queryStudents = (data) => {
	return request({
		url: '/student/query',
		method: 'post',
		data
	})
}

/**
 * 按学号查询学生信息（公开接口）
 * @param {number} stuId - 学号
 */
export const getStudentById = (stuId) => {
	return request({
		url: `/student/getById/${stuId}`,
		method: 'get'
	})
}

/**
 * 更新学生信息（需要管理员权限）
 * @param {Object} data - 学生对象
 */
export const updateStudent = (data) => {
	return request({
		url: '/student/update',
		method: 'post',
		data
	})
}

/**
 * 删除学生（需要管理员权限）
 * @param {number} stuId - 学号
 */
export const deleteStudent = (stuId) => {
	return request({
		url: `/student/delete?stuId=${stuId}`,
		method: 'post'
	})
}

/**
 * 添加学生
 */
export const addStudent = (data) => {
	return request({
		url: '/student/add',
		method: 'post',
		data
	})
}
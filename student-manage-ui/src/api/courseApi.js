// src/api/course.js
import request from './request'

// 获取所有课程
export const getAllCourses = () => {
	return request({
		url: '/course/getAll',
		method: 'get'
	})
}

// 按条件查询课程
export const queryCourses = (data) => {
	return request({
		url: '/course/query',
		method: 'post',
		data
	})
}

// 根据 ID 获取课程
export const getCourseById = (courseId) => {
	return request({
		url: `/course/getById/${courseId}`,
		method: 'get'
	})
}

// 添加课程
export const addCourse = (data) => {
	return request({
		url: '/course/add',
		method: 'post',
		data
	})
}

// 更新课程
export const updateCourse = (data) => {
	return request({
		url: '/course/update',
		method: 'post',
		data
	})
}

/**
 * 删除课程（需要管理员权限）
 * @param {number} courseId - 课程ID
 */
export const deleteCourse = (courseId) => {
	return request({
		url: `/course/delete?courseId=${courseId}`,
		method: 'post'
	})
}

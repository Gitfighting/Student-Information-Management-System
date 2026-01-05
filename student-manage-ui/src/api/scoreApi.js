import request from './request'

// 查询所有成绩
export const getAllScores = () => {
	return request({
		url: '/score/getAll',
		method: 'get'
	})
}

// 根据条件查询成绩
export const queryScores = (data) => {
	return request({
		url: '/score/query',
		method: 'post',
		data
	})
}

// 根据ID查询成绩
export const getScoreById = (scoreId) => {
	return request({
		url: `/score/getById/${scoreId}`,
		method: 'get'
	})
}

// 添加成绩
export const addScore = (data) => {
	return request({
		url: '/score/add',
		method: 'post',
		data
	})
}

// 更新成绩
export const updateScore = (data) => {
	return request({
		url: '/score/update',
		method: 'post',
		data
	})
}

// 删除成绩
export const deleteScore = (scoreId) => {
	return request({
		url: `/score/delete?scoreId=${scoreId}`,
		method: 'post'
	})
}
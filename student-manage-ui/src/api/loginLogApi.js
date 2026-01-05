import request from './request'
// 查询所有日志
export const getAllLogs = () => {
	return request({	
		url: '/loginLog/getAll',
		method: 'get'
	})
}

// 按条件查询日志
export const queryLogs = (params) => {
	return request({
		url: '/loginLog/query',
		method: 'post',
		data: params
	})
}
if redis.call('get', KEYS[1]) == ARGV[2] then
	redis.call('set', KEYS[1], ARGV[1])
	return 1
else
	return 0
end
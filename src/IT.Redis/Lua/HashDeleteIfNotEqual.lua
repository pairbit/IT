if redis.call('hget', KEYS[1], ARGV[1]) ~= ARGV[2] then
	return redis.call('hdel', KEYS[1], ARGV[1])
else
	return 0
end
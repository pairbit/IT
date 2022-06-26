if redis.call('hget', KEYS[1], ARGV[1]) ~= ARGV[3] then
	redis.call('hset', KEYS[1], ARGV[1], ARGV[2])
	return 1
else
	return 0
end
local val = redis.call('hget', KEYS[1], ARGV[1])
if val ~= false then
	redis.call('hdel', KEYS[1], ARGV[1])
end
return val
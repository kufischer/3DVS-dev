create:
	docker build - <$(DF)

run:
	docker run -p 8181:8181 -p 8081:8081 -p 34837:34837 -p 80:80 -it $(IMAGE)

rund:
	docker run -p 8181:8181 -p 8081:8081 -p 34837:34837 -p 80:80 -d $(IMAGE)

remove:
	docker rmi -f $(IMAGE)

remove-c:
	docker rm -f $(IMAGE)

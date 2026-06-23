FROM nginxinc/nginx-unprivileged:1.27.5-alpine3.21

COPY ./maintenance_page/nginx.conf /etc/nginx/nginx.conf
COPY ./maintenance_page/html/ /usr/share/nginx/html

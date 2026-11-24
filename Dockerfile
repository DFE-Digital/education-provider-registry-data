FROM nginx/nginx-unprivileged:1.30.7-alpine3.24

COPY ./maintenance_page/nginx.conf /etc/nginx/nginx.conf
COPY ./maintenance_page/html/ /usr/share/nginx/html

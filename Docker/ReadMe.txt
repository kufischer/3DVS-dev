The Makefile works only in a Unix/Linux environment. However, for
users not really familiar with Docker it might be still helpful to see
which commands could be used in a Windows environment.

To create docker images from the provided docker files use any of the
following commands:

make create DF=SmartFactory/3DVS-SF

make create DF=Whirlpool/3DVS-WP

make create DF=BoschRexroth/3DVS-BR

or

make create DF=generic/3DVS2

To run the created images use:

make run IMAGE=<image>

or

make rund IMAGE=<image>

Use "docker images" to look for the image id (<image>).  run starts
the Docker image in interactive mode, to make interaction with the
FiVES server possible. rund starts the Docker image as a demon without
tty connection.

To delete an image use:

make remove IMAGE=<image>

In case the image was started as a demon use

make remove-c IMAGE=<container-id>

to delete the container.

In these commands <image> needs to be replaced by the IMAGE ID. IMAGE
ID is displayed in the last line of the creation output or can be
displayed by the Docker command "docker images".

When the Docker image is run it should be possible to connect to
3DVS with Google Chrome or Mozilla Firefox using the URL:

http://localhost/WebClient/client.xhtml

http://localhost/SmartFactory/assembly.html

http://localhost/WebClientBR/client.xhtml

or

http://localhost/WebClientWp/client.html

respectively. If port 80 is already busy on the host machine the port
mapping needs to be changed in the "docker run" command. However, in
this case "localhost" needs to be changed to "localhost:<port>" where
<port> needs to be replaced by the respective port number.

No user credentials are needed for singing in. Just click on the
respectively button. Zoom in on the models is possible with dragging
the mouse while doing a right click; rotating with left mouse button.

Please send email to Klaus.Fischer@dfki.de if you have any questions
or comments.

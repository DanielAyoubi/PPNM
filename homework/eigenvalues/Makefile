DIR=../matlib

all: Out.txt plot_eigenfunctions.svg plot_dr.svg plot_rmax.svg

Out.txt: main.exe
	mono main.exe > Out.txt

plot_eigenfunctions.svg : plot_eigenfunctions.gpi eigenfunctions.data 
	gnuplot $<

plot_dr.svg : plot_dr.gpi different_dr.data | plot_dr.gpi
	gnuplot $<

plot_rmax.svg : plot_rmax.gpi different_rmax.data
	gnuplot $<

different_dr.data different_rmax.data : main.exe

eigenfunctions.data : different_dr.data different_rmax.data

main.exe: main.cs matrix.dll 
	mcs main.cs $(addprefix -reference:,$(filter %.dll,$^))

matrix.dll : $(DIR)/vector.cs $(DIR)/matrix.cs jacobi.cs hydrogen_atom.cs
	mcs $^ -target:library -out:matrix.dll

hydrogen_atom.exe : hydrogen_atom.cs matrix.dll
	mcs hydrogen_atom.cs -out:$@

clean:
	rm -f *.dll *.exe *.svg *.data Out*



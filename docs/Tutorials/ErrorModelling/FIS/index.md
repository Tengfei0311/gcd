---
title: Building your own FIS
---

### Customizing Fuzzy Inference Systems using Matlab Fuzzy Logic Toolbox

These video tutorials goes through how to customize a Fuzzy Inference System in Matlab for use in the GCD. Note that these videos are only useful if you have Matlab installed with  the [Fuzzy Logic Toolbox](http://www.mathworks.com/products/fuzzylogic/). If you want to make your own fuzzy inference systems or edit systems you can still do so (scroll down to 'Customizing Fuzzy Inference Systems with a Text Editor').

#### Part I - Getting Around in the Fuzzy Logic Toolobox

<iframe width="560" height="315" src="https://www.youtube.com/embed/USy-Zk5wNuw" frameborder="0" gesture="media" allow="encrypted-media" allowfullscreen></iframe>

#### Part II -Looking at an Existing Fuzzy Inference System

This goes through how an existing FIS that is installed with GCD 5 is set up, which is described in both Wheaton (2008) and Wheaton et al. (2010). The video shows you how to explore how the fuzzy inference system works and how it is organized.

<iframe width="560" height="315" src="https://www.youtube.com/embed/mOYfYoNxRTY" frameborder="0" gesture="media" allow="encrypted-media" allowfullscreen></iframe>

#### Part III - Modifying a Fuzzy Inference System

This video goes through modifying inputs and rules in Matlab's [Fuzzy Logic Toolbox](http://www.mathworks.com/products/fuzzylogic/).

<iframe width="560" height="315" src="https://www.youtube.com/embed/ld1Q3uEo1SQ" frameborder="0" gesture="media" allow="encrypted-media" allowfullscreen></iframe>

### Customizing Fuzzy Inference Systems with a Text Editor

This video goes through how you customize a Fuzzy Inference System using either GCD or a text editor for use in GCD.

<iframe width="560" height="315" src="https://www.youtube.com/embed/sPDx8Wsu2DA" frameborder="0" gesture="media" allow="encrypted-media" allowfullscreen></iframe>

To see how to edit an FIS using the GCD's ```Fuzzy Inference System Editor```, see here.

### Running your own FIS with more than Three Inputs

In GCD 5, we currently only support running fuzzy inference systems with two or three inputs. If you have more then three inputs, you'll need to run your inference system outside of the GCD. You can still load your FIS output as a user-specified error grid in the [Survey Library]({{ site.baseurl }}/system/errors/NodeNotFound?suri=wuid:gx:3ed05905e41de6f6), after converting it to raster format GCD recognizes (e.g. `*.tiff` or `*.img`). 

As a work around, we've developed some simple Matlab code (`FIS_IT.m`) that allows you to build spatial FIS outputs from an FIS you have created, with anywhere between two and seven inputs. You need Matlab's fuzzy logic toolbox files loaded to use the code. The code can be downloaded [here](http://etal.usu.edu/GCD/FIS-IT-Program.zip). 
​    [`FIS-IT-Program.zip`](http://etal.usu.edu/GCD/FIS-IT-Program.zip)

This video tutorial goes through how to run the FIS it! code:

<iframe width="560" height="315" src="https://www.youtube.com/embed/UmxxHcO_NcM" frameborder="0" gesture="media" allow="encrypted-media" allowfullscreen></iframe>

For those of you who might want to extend the code, this video tutorial walks through how the code is organized and works:

<iframe width="560" height="315" src="https://www.youtube.com/embed/yPDJKem3GnI" frameborder="0" gesture="media" allow="encrypted-media" allowfullscreen></iframe>

If you do make your own FIS, and want to produce some production quality plots of the input and output membership functions, check out '[R Plot Function for Fuzzy Membership Functions]({{ site.baseurl }}/tutorials--how-to/viii-building-your-own-fis/fuzzymembershipplot)'.

### Further Reading on Fuzzy Inference Systems

See pages 97-108 of: 

- Chapter 4 of Wheaton JM. 2008. [Uncertainty in Morphological Sediment Budgeting of Rivers](http://www.joewheaton.org/Home/research/projects-1/morphological-sediment-budgeting/phdthesis). Unpublished PhD Thesis, University of Southampton, Southampton, 412 pp.

See page 142-146 of:
- Wheaton JM, Brasington J, Darby SE and Sear D. 2010. [Accounting for Uncertainty in DEMs from Repeat Topographic Surveys: Improved Sediment Budgets](http://dx.doi.org/10.1002/esp.1886). Earth Surface Processes and Landforms. 35 (2): 136-156. DOI: [10.1002/esp.1886](http://dx.doi.org/10.1002/esp.1886).

- 2016.  Bangen S‡ , Hensleigh J‡, McHugh P, and Wheaton JM.  [Error modeling of DEMs from topographic surveys of rivers using Fuzzy Inference Systems](https://www.researchgate.net/publication/292210478_Error_modeling_of_DEMs_from_topographic_surveys_of_rivers_using_fuzzy_inference_systems).  Water Resources Research. DOI: [10.1002/2015WR018299](http://dx.doi.org/10.1002/2015WR018299).
- 
[Matlab Fuzzy Logic Toolbox Documentation](http://www.mathworks.com/help/toolbox/fuzzy/)

------
<div align="center">
	<a class="hollow button" href="{{ site.baseurl }}/Help"><i class="fa fa-chevron-circle-left"></i>  Back to GCD Help </a>  
	<a class="hollow button" href="{{ site.baseurl }}/"><img src="{{ site.baseurl}}/assets/images/icons/GCDAddIn.png">  Back to GCD Home </a>  
</div>
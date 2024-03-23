import React, { useEffect, useRef, useState } from 'react';
import PropTypes from 'prop-types';
import { useDataStreamContext } from '../providers/DataStreamProvider';


const DataDiagramController = () => {
    const dataStreamContext = useDataStreamContext();
    
    const valueState = {}
    for (const [deviceId, deviceState] of Object.entries(dataStreamContext.deviceStates)) {
        valueState[deviceState.drawingId + "-VAL"] = deviceState.value;
        valueState[deviceState.drawingId + "-MIN"] = deviceState.valueRunMinimum;
        valueState[deviceState.drawingId + "-MAX"] = deviceState.valueRunMaximum;
    }

    return <SvgDiagram valueState={valueState} />
}

const SvgDiagram = ({ valueState: valueMap }) => {
    const svgContainerRef = useRef(null);
    const [svgLoaded, setSvgLoaded] = useState(false)

    // Load the SVG from an external source
    useEffect(() => {
        const loadSvg = async () => {
            try {
                const response = await fetch('/diagram.svg',  {cache: "no-store"});
                const svgText = await response.text();
                svgContainerRef.current.innerHTML = svgText;
                setSvgLoaded(true)
            } catch (error) {
                console.error('Error loading SVG:', error);
            }
        };
        loadSvg();
    }, []); // Empty dependency array means this effect runs once on mount

    const updateElement = (svg, id, value) => {
        // console.log(`${id}: ${value}`);
        const selector = '#' + id;
        const element = svg.querySelector(selector);
        
        if (!element) {
            console.warn("Could not find element with selector '" + selector + "' is non-existent");
            return;
        }
        element.textContent = value.toFixed(2)
    }

    // Update the SVG's color based on the fillColor prop
    useEffect(() => {
        if (!svgLoaded) {
            // Not ready
            return
        }
        const svgElement = svgContainerRef.current.querySelector('svg');

        if (svgElement) {
            for (const [id, value] of Object.entries(valueMap)) {
                updateElement(svgElement, id, value)
            }
        } else {
            console.warn("No SVG element")
        }

    }, [svgLoaded, valueMap]); // Redraw when SVG is loaded or 

    return <div ref={svgContainerRef}></div>;
}


SvgDiagram.propTypes = {
    valueState: PropTypes.object,
};

export default DataDiagramController;
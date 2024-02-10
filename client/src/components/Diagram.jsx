
// https://datalanguage.com/blog/graphical-uis-with-svg-and-react-part-1-declarative-graphics
import PropTypes from 'prop-types';
import { useDataStreamContext } from '../providers/DataStreamProvider';

const basicProps = {
    x: PropTypes.node.isRequired,
    y: PropTypes.node.isRequired,
    // width: PropTypes.node.isRequired,
    // height: PropTypes.node.isRequired,
}

const LabelledBlock = ({name, value, x, y}) => {
    const textWidth = name.length * 10;
    const textHeight = 40;
    return (
        <g>
            <text x={x} y={y - 8}  textAnchor='left' dominantBaseline='bottom'>{name}</text>
            <rect x={x} y={y} width={textWidth} height={textHeight} rx="10" ry="10" style={{"fill":"white", "stroke":"black"}} />
            <text x={x + textWidth / 2} y={y + textHeight/2}  textAnchor='middle' dominantBaseline='middle'>{value}</text>
        </g>
    )
}

LabelledBlock.propTypes = {
    ...basicProps,
    name: PropTypes.node.isRequired,
    value: PropTypes.node.isRequired,

};

const FixedDevices = [
    {
        drawingId: "Sensor1",
        location: {
            x : 80,
            y: 80
        }
    },
    {
        drawingId: "string",
        location: {
            x : 80,
            y: 180
        }
    },
]

const Diagram = () => {

    const dataStreamContext = useDataStreamContext();
    const width = 800;
    const height = 600;
    return (
        <div >
            <svg width={width} height={height} viewBox={`0 0 ${width} ${height}`} style={{backgroundColor: "blue"}}>
                <circle cx={40} cy={40} r={30} fill="red" />
                {FixedDevices.map((device) => {
                    if (device.drawingId in dataStreamContext.devices) {
                        return (<LabelledBlock key={device.drawingId} 
                            name={device.drawingId}
                            value={dataStreamContext.devices[device.drawingId]} 
                            x={device.location.x} 
                            y={device.location.y} 
                        />);
                    }
                })}
                {/* <LabelledBlock name="Device Name" value="Hallelujah" x={80} y={80} />
                <LabelledBlock name="MyDevice" value={dataStreamContext.devices["MyDevice"]} x={80} y={180} /> */}
            </svg>
        </div>
    )

}


export default Diagram;